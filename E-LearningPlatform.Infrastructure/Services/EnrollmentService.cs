using AutoMapper;
using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.ContinueLearning;
using E_LearningPlatform.Application.DTOs.Enrollment;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public EnrollmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task EnrollAsync(
            int userId,
            int courseId,
            Money price,
            CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == courseId,
                    cancellationToken);

            if (course == null)
            {
                throw new InvalidOperationException(
                    "Course not found.");
            }

            if (!course.IsPublished || !course.IsActive)
            {
                throw new InvalidOperationException(
                    "Course is unavailable.");
            }

            var alreadyEnrolled = await _unitOfWork.Enrollments
                .Query()
                .AnyAsync(
                    x => x.UserId == userId &&
                         x.CourseId == courseId,
                    cancellationToken);

            if (alreadyEnrolled)
            {
                throw new InvalidOperationException(
                    "User already enrolled.");
            }

            var enrollment = new CourseEnrollment(
                courseId,
                userId,
                price);

            await _unitOfWork.Enrollments
                .AddAsync(enrollment, cancellationToken);

            course.IncrementEnrollmentCount();
        }

        public async Task<PagedResult<EnrollmentCourseResponseDto>>
            GetUserEnrollmentsAsync(
            int userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");
            }

            if (userId != _currentUserService.UserId.Value)
            {
                throw new UnauthorizedAccessException(
                    "You can only access your own enrollments.");
            }

            if (pageNumber <= 0)
            {
                throw new ArgumentException(
                    "Page number must be greater than zero.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException(
                    "Page size must be greater than zero.");
            }

            var enrollmentsQuery = _unitOfWork.Enrollments
                .Query()
                .Where(e => e.UserId == userId)
                .Include(e => e.Course);

            var totalCount = await enrollmentsQuery
                .CountAsync(cancellationToken);

            var enrollments = await enrollmentsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var enrollmentDtos =
                _mapper.Map<List<EnrollmentCourseResponseDto>>(
                    enrollments);

            return new PagedResult<EnrollmentCourseResponseDto>
            {
                Items = enrollmentDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> IsUserEnrolledAsync(
            int userId,
            int courseId,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Enrollments
                .Query()
                .AnyAsync(
                    x => x.UserId == userId &&
                         x.CourseId == courseId,
                    cancellationToken);
        }

        public Task<EnrollmentCourseResponseDto>
            GetEnrollmentAsync(
            int enrollmentId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task CompleteEnrollmentAsync(
            int enrollmentId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeactivateEnrollmentAsync(
            int enrollmentId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<ContinueLearningResponseDto>> GetContinueLearningAsync(CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");
            }
            var userId = _currentUserService.UserId.Value;
            var enrollments = await _unitOfWork.Enrollments
                .Query()
                .Include(e => e.Course)
                .Where(x => x.UserId == userId &&
                x.ProgressPercent > 0 &&
                !x.IsCompleted)
                .OrderByDescending(x => x.LastAccessedAt)
                .ToListAsync(cancellationToken);
            if (!enrollments.Any())
            {
                return new PagedResult<ContinueLearningResponseDto>
                {
                    Items = new List<ContinueLearningResponseDto>(),
                    TotalCount = 0,
                    PageNumber = 1,
                    PageSize = 0
                };
            }
            var results = new List<ContinueLearningResponseDto>();
            foreach (var enrollment in enrollments)
            {
                var lastProgress = await _unitOfWork.LessonsProgress
                .Query()
                .Include(p => p.Lesson)
                .ThenInclude(l => l.Section)
                .Where(x =>
                x.UserId == userId &&
                x.Lesson.Section.CourseId == enrollment.CourseId
                ).OrderByDescending(x => x.UpdatedAtUtc)
                .FirstOrDefaultAsync(cancellationToken);
                if (lastProgress == null)
                {
                    continue;
                }

                results.Add(new ContinueLearningResponseDto
                {
                    CourseId = enrollment.CourseId,
                    CourseTitle = enrollment.Course.Title,
                    CourseSlug = enrollment.Course.Slug.Value,
                    CourseThumbnailUrl = enrollment.Course.Thumbnail.Url,
                    ProgressPercent = enrollment.ProgressPercent,
                    LastLessonId = lastProgress.LessonId,
                    LastLessonTitle = lastProgress.Lesson.Title,
                    LastAccessedAt =enrollment.LastAccessedAt ?? enrollment.EnrolledAt
                });
            }

            return new PagedResult<ContinueLearningResponseDto>
            {
                Items = results,
                TotalCount = results.Count,
                PageNumber = 1,
                PageSize = results.Count
            };
        }
    }
}