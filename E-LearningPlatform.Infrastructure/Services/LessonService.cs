using AutoMapper;
using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Lesson;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class LessonService : ILessonService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICourseAuthorizationService _courseAuthorizationService;
        public LessonService (IUnitOfWork unitOfWork,
            IMapper mapper,
            ICloudinaryService cloudinaryService,
            ICurrentUserService currentUserService,
            ICourseAuthorizationService courseAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
            _currentUserService = currentUserService;
            _courseAuthorizationService = courseAuthorizationService;
        }

        public async Task<LessonCreatedResponseDto> AddLessonAsync (AddLessonRequestDto request)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(request.SectionId) ??
                throw new NotFoundException($"Section with ID {request.SectionId} not found.");
            var course = await _unitOfWork.Courses.GetByIdAsync(section.CourseId) ??
                throw new NotFoundException($"Course with ID {section.CourseId} not found.");
            await _courseAuthorizationService.EnsureInstructorOwnsCourseAsync(course.Id);
            var lessonCount = await _unitOfWork.Lessons
                   .Query()
                   .Where(l => l.SectionId == request.SectionId)
                   .CountAsync();
            var lessonOrder = lessonCount + 1;
            var lesson = new Lesson(
       request.Title,
       lessonOrder,
       request.SectionId,
       request.ContentType,
       new Duration(request.DurationInMinutes)
   );
            lesson.MarkPreview(request.IsPreview);
            if (request.ReleaseDate.HasValue)
                lesson.ScheduleRelease(request.ReleaseDate.Value);

            if (request.VideoFile != null)
            {
                using var stream = request.VideoFile.OpenReadStream();
                var uploadResult = await _cloudinaryService.UploadVideoAsync(stream, request.VideoFile.FileName);
                lesson.SetVideoUrl(uploadResult.Url);
                var media = new Media(
            uploadResult.PublicId,
            uploadResult.Url,
MapToMediaCategory(request.ContentType),
MediaType.Video,
            request.VideoFile.Length,
            request.VideoFile.ContentType,
            _currentUserService.UserId!.Value
        );

                await _unitOfWork.Media.AddAsync(media);
            }

            await _unitOfWork.Lessons.AddAsync(lesson);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LessonCreatedResponseDto>(lesson);
        }


        public async Task DeleteLessonAsync (int lessonId, int sectionId)
        {
            if (lessonId <= 0)
                throw new BadRequestException("Invalid lesson id.");

            if (sectionId <= 0)
                throw new BadRequestException("Invalid section id.");

            var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);

            if (lesson == null)
                throw new NotFoundException("Lesson not found.");

            if (lesson.SectionId != sectionId)
                throw new BadRequestException("Lesson does not belong to this section.");

            var section = await _unitOfWork.Sections.GetByIdAsync(sectionId);

            if (section == null)
                throw new NotFoundException("Section not found.");

            var course = await _unitOfWork.Courses.GetByIdAsync(section.CourseId);

            if (course == null)
                throw new NotFoundException("Course not found.");

            await _courseAuthorizationService.EnsureInstructorOwnsCourseAsync(course.Id);

            var deletedOrder = lesson.Order;

            _unitOfWork.Lessons.Remove(lesson);

            var mediaList = await _unitOfWork.Media
                .Query()
                .Where(m => m.LessonId == lessonId)
                .ToListAsync();

            foreach (var media in mediaList)
            {
                media.MarkDeleted();

                await _cloudinaryService.DeleteAsync(media.PublicId);
            }

            var lessonsToUpdate = await _unitOfWork.Lessons
               .Query()
               .Where(l =>
                   l.SectionId == sectionId &&
                   l.Order > deletedOrder)
               .ToListAsync();

            foreach (var l in lessonsToUpdate)
            {
                l.SetOrder(l.Order - 1);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<LessonCreatedResponseDto> UpdateLessonAsync (int lessonId, AddLessonRequestDto request)
        {
            if (lessonId <= 0)
                throw new ArgumentException("Invalid lesson ID.", nameof(lessonId));

            var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId) ??
                throw new NotFoundException($"Lesson with ID {lessonId} not found.");

            if (lesson.SectionId != request.SectionId)
                throw new BadRequestException("Lesson does not belong to the specified section.");

            var section = await _unitOfWork.Sections.GetByIdAsync(request.SectionId) ??
                 throw new NotFoundException($"Section with ID {request.SectionId} not found.");

            var course = await _unitOfWork.Courses.GetByIdAsync(section.CourseId) ??
                 throw new NotFoundException($"Course with ID {section.CourseId} not found.");
            await _courseAuthorizationService.EnsureInstructorOwnsCourseAsync(course.Id);

            lesson.SetTitle(request.Title);

            lesson.MarkPreview(request.IsPreview);

            if (request.ReleaseDate.HasValue)
                lesson.ScheduleRelease(request.ReleaseDate.Value);

            if (request.VideoFile != null)
            {
                var oldMedia = await _unitOfWork.Media
                    .Query()
                    .Where(m =>
                        m.LessonId == lesson.Id &&
                        m.Category == MediaCategory.LessonVideo)
                    .ToListAsync();

                foreach (var mediaItem in oldMedia)
                {
                    mediaItem.MarkDeleted();

                    await _cloudinaryService.DeleteAsync(mediaItem.PublicId);
                }

                using var stream = request.VideoFile.OpenReadStream();

                var uploadResult = await _cloudinaryService
                    .UploadVideoAsync(stream, request.VideoFile.FileName);

                lesson.SetVideoUrl(uploadResult.Url);

                var mediaCategory = MapToMediaCategory(request.ContentType);

                var media = new Media(
                    uploadResult.PublicId,
                    uploadResult.Url,
                    mediaCategory,
                    MediaType.Video,
                    request.VideoFile.Length,
                    request.VideoFile.ContentType,
                    _currentUserService.UserId!.Value
                );

                await _unitOfWork.Media.AddAsync(media);
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LessonCreatedResponseDto>(lesson);
        }
        private static MediaCategory MapToMediaCategory (LessonContentType contentType)
        {
            return contentType switch
            {
                LessonContentType.Video => MediaCategory.LessonVideo,

                LessonContentType.Downloadable => MediaCategory.LessonMaterial,

                LessonContentType.Article => MediaCategory.LessonMaterial,

                LessonContentType.Quiz => throw new InvalidOperationException(
                    "Quiz lessons do not support media files."),

                _ => throw new ArgumentOutOfRangeException(nameof(contentType))
            };

        }

        public async Task ReorderLessonsAsync (int sectionId, List<int> orderedLessonIds)
        {
            if (sectionId <= 0)
                throw new BadRequestException("Invalid section id.");
            if (orderedLessonIds == null || !orderedLessonIds.Any())

                throw new BadRequestException("Ordered lesson IDs cannot be null or empty.");
            var section = await _unitOfWork.Sections.GetByIdAsync(sectionId) ??
                throw new NotFoundException($"Section with ID {sectionId} not found.");
            var course = await _unitOfWork.Courses.GetByIdAsync(section.CourseId) ??
            throw new NotFoundException($"Course with ID {section.CourseId} not found.");
            await _courseAuthorizationService.EnsureInstructorOwnsCourseAsync(course.Id);
            var lessons = await _unitOfWork.Lessons
               .Query()
               .Where(l => l.SectionId == sectionId)
               .ToListAsync();
            if (lessons.Count != orderedLessonIds.Count ||
                lessons.Any(l => !orderedLessonIds.Contains(l.Id)))
            {
                throw new BadRequestException("Ordered lesson IDs do not match the existing lessons in the section.");
            }
            for (int i = 0; i < orderedLessonIds.Count; i++)
            {
                var lessonId = orderedLessonIds[i];
                var lesson = lessons.First(l => l.Id == lessonId);
                lesson.SetOrder(i + 1);
            }
            await _unitOfWork.SaveChangesAsync();

        }

        public Task<LessonDetailsResponseDto> GetLessonByIdAsync (int lessonId)
        {
            if (lessonId <= 0)
                throw new BadRequestException("Invalid lesson id.");
            var lesson = _unitOfWork.Lessons.GetByIdAsync(lessonId) ??
                throw new NotFoundException($"Lesson with ID {lessonId} not found.");
            return Task.FromResult(_mapper.Map<LessonDetailsResponseDto>(lesson));
        }

        public Task<PagedResult<LessonDetailsResponseDto>> GetLessonsBySectionIdAsync (int sectionId, PagedRequest PagedRequest)
        {
            if (sectionId <= 0)
                throw new BadRequestException("Invalid section id.");
            var section = _unitOfWork.Sections.GetByIdAsync(sectionId) ??
                throw new NotFoundException($"Section with ID {sectionId} not found.");
            var lessonsQuery = _unitOfWork.Lessons
                .Query()
                .Where(l => l.SectionId == sectionId)
                .OrderBy(l => l.Order);
            var totalCount = lessonsQuery.Count();
            var lessons = lessonsQuery
                .Skip((PagedRequest.PageNumber - 1) * PagedRequest.PageSize)
                .Take(PagedRequest.PageSize)
                .ToList();
            var lessonDtos = _mapper.Map<List<LessonDetailsResponseDto>>(lessons);
            var pagedResult = new PagedResult<LessonDetailsResponseDto>
            {
                Items = lessonDtos,
                TotalCount = totalCount,
                PageNumber = PagedRequest.PageNumber,
                PageSize = PagedRequest.PageSize
            };
            return Task.FromResult(pagedResult);
        }
    }
}




