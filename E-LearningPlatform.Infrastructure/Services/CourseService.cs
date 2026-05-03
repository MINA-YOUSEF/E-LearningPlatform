using AutoMapper;
using E_LearningPlatform.Application.DTOs.Course;
using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;


namespace E_LearningPlatform.Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public CourseService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ICloudinaryService cloudinaryService,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
            _currentUserService = currentUserService;
        }
        public async Task<CourseCreatedResponseDto> AddCourseAsync(AddCourseRequestDto request)
        {
            var userId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException(
                    "User must be authenticated to create a course.");

            var course = _mapper.Map<Course>(request)
                ?? throw new Exception("Failed to map course.");

            if (request.ThumbnailFile != null)
            {
                using var stream = request.ThumbnailFile.OpenReadStream();

                var uploadResult = await _cloudinaryService
                    .UploadImageAsync(stream, request.ThumbnailFile.FileName);

                var thumbnailMedia = new Media(
                    uploadResult.PublicId,
                    uploadResult.Url,
                    MediaCategory.CourseThumbnail,
                    MediaType.Image,
                    request.ThumbnailFile.Length,
                    request.ThumbnailFile.ContentType,
                    userId
                );

                await _unitOfWork.Media.AddAsync(thumbnailMedia);

                course.SetThumbnail(thumbnailMedia.Id);
            }

            course.InstructorId = userId;


            await _unitOfWork.Courses.AddAsync(course);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CourseCreatedResponseDto>(course)!;
        }

        public async Task<CourseCreatedResponseDto> GetCourseByIdAsync(int courseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId)
                ?? throw new KeyNotFoundException($"Course with ID {courseId} not found.");
            return _mapper.Map<CourseCreatedResponseDto>(course)!;
        }

        public async Task<CourseCreatedResponseDto> UpdateCourseAsync(int courseId, AddCourseRequestDto request)
        {
            if (courseId <= 0)
                throw new ArgumentException("Course ID must be greater than zero.", nameof(courseId));
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            var result = await _unitOfWork.Courses.GetByIdAsync(courseId)
                 ?? throw new KeyNotFoundException($"Course with ID {courseId} not found.");
            var course = _mapper.Map(request, result);
            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CourseCreatedResponseDto>(course)!;
        }
    }
}
