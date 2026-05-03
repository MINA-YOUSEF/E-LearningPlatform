using AutoMapper;
using CloudinaryDotNet;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using Org.BouncyCastle.Pqc.Asn1;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class MediaService : IMediaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICloudinaryService _cloudinaryService;

        public MediaService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ICloudinaryService cloudinaryService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _cloudinaryService = cloudinaryService;
        }

        public async Task DeleteMediaAsync(int mediaId, CancellationToken cancellationToken = default)
        {
            var media = await _unitOfWork.Media.GetByIdAsync(mediaId);
            if (media == null)
                throw new NotFoundException("Media not found.");
            if (media.UploadedByUserId != _currentUserService.UserId)
                throw new ForbiddenException("You are not allowed to delete this media.");
            _unitOfWork.Media.Remove(media);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        public async Task<Media> ReplaceLessonMediaAsync(
            int lessonId,
             IFormFile file,
              LessonContentType contentType,
               CancellationToken cancellationToken = default)
        {

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file.");
            }
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);
            if (lesson == null)
                throw new NotFoundException("Lesson not found.");

            var section = await _unitOfWork.Sections.GetByIdAsync(lesson.SectionId);
            if (section == null)
                throw new NotFoundException("Section not found.");
            var course = await _unitOfWork.Courses.GetByIdAsync(section.CourseId);
            if (course == null)
                throw new NotFoundException("Course not found.");
            if (course.InstructorId != _currentUserService.UserId)
                throw new ForbiddenException("You are not allowed to modify media for this lesson.");
            var mediaCategory = MediaHelper.MapToMediaCategory(contentType);

            // get old media
            var oldMediaList = await _unitOfWork.Media.Query()
                .Where(m => m.LessonId == lessonId && m.Category == mediaCategory)
                .ToListAsync(cancellationToken);

            foreach (var old in oldMediaList)
            {
                await _cloudinaryService.DeleteAsync(old.PublicId, cancellationToken);
                old.MarkDeleted();
            }

            // upload new
            Media newMedia;

            switch (contentType)
            {
                case LessonContentType.Video:
                    {
                        using var stream = file.OpenReadStream();

                        var result = await _cloudinaryService.UploadVideoAsync(stream, file.FileName, cancellationToken);

                        lesson.SetVideoUrl(result.Url);

                        newMedia = new Media(result.PublicId, result.Url, mediaCategory,
                            MediaType.Video, file.Length, file.ContentType ?? "application/octet-stream",
                            _currentUserService.UserId!.Value);

                        break;
                    }

                case LessonContentType.Downloadable:
                    {
                        using var stream = file.OpenReadStream();

                        var result = await _cloudinaryService.UploadFileAsync(stream, file.FileName, cancellationToken);

                        newMedia = new Media(result.PublicId, result.Url, mediaCategory,
                            MediaType.Image, file.Length, file.ContentType ?? "application/octet-stream",
                            _currentUserService.UserId!.Value);

                        break;
                    }

                case LessonContentType.Article:
                    {
                        using var stream = file.OpenReadStream();

                        var result = await _cloudinaryService.UploadImageAsync(stream, file.FileName, cancellationToken);

                        newMedia = new Media(result.PublicId, result.Url, mediaCategory,
                            MediaType.Image, file.Length, file.ContentType ?? "application/octet-stream",
                            _currentUserService.UserId!.Value);

                        break;
                    }

                default:
                    throw new InvalidOperationException("Unsupported content type.");
            }

            newMedia.SetLessonId(lessonId);

            await _unitOfWork.Media.AddAsync(newMedia, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newMedia;
        }
        public async Task<Media> SetCourseThumbnailAsync(
            int courseId,
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("Invalid file.");

            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);

            if (course == null)
                throw new NotFoundException("Course not found.");

            if (course.InstructorId != _currentUserService.UserId)
                throw new ForbiddenException("You are not allowed to modify this course.");

            var oldMedia = await _unitOfWork.Media.Query()
               .Where(m => m.Id == course.ThumbnailId)
               .FirstOrDefaultAsync(cancellationToken);

            if (oldMedia != null)
            {
                await _cloudinaryService.DeleteAsync(oldMedia.PublicId, cancellationToken);
                oldMedia.MarkDeleted();
            }

            using var stream = file.OpenReadStream();

            var result = await _cloudinaryService
                .UploadImageAsync(stream, file.FileName, cancellationToken);

            if (result == null || string.IsNullOrEmpty(result.Url))
                throw new InvalidOperationException("Failed to upload image.");

            var media = new Media(
               result.PublicId,
               result.Url,
               MediaCategory.CourseThumbnail,
               MediaType.Image,
               file.Length,
               file.ContentType ?? "application/octet-stream",
               _currentUserService.UserId!.Value
           );

            await _unitOfWork.Media.AddAsync(media, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            course.SetThumbnail(media.Id);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return media;
        }

        public async Task<Media> UploadLessonMediaAsync(
    int lessonId,
    IFormFile file,
    LessonContentType contentType,
    CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("Invalid file.");

            var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);

            if (lesson == null)
                throw new NotFoundException("Lesson not found.");

            var section = await _unitOfWork.Sections.GetByIdAsync(lesson.SectionId);

            if (section == null)
                throw new NotFoundException("Section not found.");

            var course = await _unitOfWork.Courses.GetByIdAsync(section.CourseId);

            if (course == null)
                throw new NotFoundException("Course not found.");

            if (course.InstructorId != _currentUserService.UserId)
                throw new ForbiddenException("You are not allowed to upload media for this lesson.");

            var mediaCategory = MediaHelper.MapToMediaCategory(contentType);

            Media newMedia;

            switch (contentType)
            {
                case LessonContentType.Video:
                    {
                        using var stream = file.OpenReadStream();

                        var result = await _cloudinaryService
                            .UploadVideoAsync(stream, file.FileName, cancellationToken);

                        lesson.SetVideoUrl(result.Url);

                        newMedia = new Media(
                            result.PublicId,
                            result.Url,
                            mediaCategory,
                            MediaType.Video,
                            file.Length,
                            file.ContentType ?? "application/octet-stream",
                            _currentUserService.UserId!.Value
                        );

                        break;
                    }

                case LessonContentType.Downloadable:
                    {
                        using var stream = file.OpenReadStream();

                        var result = await _cloudinaryService
                            .UploadFileAsync(stream, file.FileName, cancellationToken);

                        newMedia = new Media(
                            result.PublicId,
                            result.Url,
                            mediaCategory,
                            MediaType.Certificate,
                            file.Length,
                            file.ContentType ?? "application/octet-stream",
                            _currentUserService.UserId!.Value
                        );

                        break;
                    }

                case LessonContentType.Article:
                    {
                        using var stream = file.OpenReadStream();

                        var result = await _cloudinaryService
                            .UploadImageAsync(stream, file.FileName, cancellationToken);

                        newMedia = new Media(
                            result.PublicId,
                            result.Url,
                            mediaCategory,
                            MediaType.Image,
                            file.Length,
                            file.ContentType ?? "application/octet-stream",
                            _currentUserService.UserId!.Value
                        );

                        break;
                    }

                default:
                    throw new InvalidOperationException("Unsupported content type.");
            }

            newMedia.SetLessonId(lessonId);

            await _unitOfWork.Media.AddAsync(newMedia, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newMedia;
        }
    }

}