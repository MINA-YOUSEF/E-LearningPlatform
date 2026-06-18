using AutoMapper;
using E_LearningPlatform.Application.DTOs.Progress;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace E_LearningPlatform.Infrastructure.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public ProgressService (IUnitOfWork unitOfWork,
           IMapper mapper,
           ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<CourseProgressResponseDto>
      GetCourseProgressAsync (
      int courseId,
      CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");
            }

            var enrollment = await _unitOfWork.Enrollments
                .Query()
                .FirstOrDefaultAsync(
                    x => x.UserId ==
                         _currentUserService.UserId &&
                         x.CourseId == courseId,
                    cancellationToken);

            if (enrollment == null)
            {
                throw new InvalidOperationException(
                    "Enrollment not found.");
            }

            return _mapper.Map<CourseProgressResponseDto>(enrollment);
        }

        public async Task<LessonProgressResponseDto> GetLessonProgressAsync (int lessonId, CancellationToken cancellationToken = default)
        {
            var lesson = await _unitOfWork.Lessons.Query().Include(x => x.Section).FirstOrDefaultAsync(x => x.Id == lessonId, cancellationToken);
            if (lesson == null)
                throw new InvalidOperationException(
                    "Lesson not found.");
            var enrolled = await _unitOfWork.Enrollments.Query().AnyAsync(x => x.CourseId == lesson.Section.CourseId && x.UserId == _currentUserService.UserId, cancellationToken);
            if (!enrolled)
            {
                throw new UnauthorizedAccessException(
                    "You are not enrolled in this course.");
            }
            var progress = await _unitOfWork.LessonsProgress.Query().FirstOrDefaultAsync(x => x.LessonId == lessonId && x.UserId == _currentUserService.UserId, cancellationToken);
            if (progress == null)
                throw new InvalidOperationException("Progress not found");
            return _mapper.Map<LessonProgressResponseDto>(progress);

        }

        public async Task MarkLessonProgressAsync (int lessonId, int watchedSeconds, bool Completed, CancellationToken cancellationToken = default)
        {
            var lesson = await _unitOfWork.Lessons.Query().Include(x => x.Section).FirstOrDefaultAsync(x => x.Id == lessonId, cancellationToken);
            if (lesson == null)
            {
                throw new InvalidOperationException(
                    "Lesson not found.");
            }
            var enrolled = await _unitOfWork.Enrollments.Query().AnyAsync(x => x.CourseId == lesson.Section.CourseId && x.UserId == _currentUserService.UserId, cancellationToken);
            if (!enrolled)
            {
                throw new UnauthorizedAccessException(
                    "You are not enrolled in this course.");
            }
            var enrollment = await _unitOfWork.Enrollments
.Query()
.FirstOrDefaultAsync(
  x => x.UserId ==
       _currentUserService.UserId &&
       x.CourseId ==
       lesson.Section.CourseId,
  cancellationToken);
            if (enrollment == null)
            {
                throw new InvalidOperationException(
                    "Enrollment not found.");
            }
            if (enrollment.IsCompleted)
            {
                throw new InvalidOperationException(
                    "Course already completed.");
            }
            var progress = await _unitOfWork.LessonsProgress.Query().FirstOrDefaultAsync(x => x.LessonId == lessonId && x.UserId == _currentUserService.UserId, cancellationToken);
            if (progress == null)
            {
                progress = new LessonProgress(
                                     lessonId,
                           _currentUserService.UserId.Value);

                await _unitOfWork.LessonsProgress
                    .AddAsync(progress, cancellationToken);

            }
            progress.MarkProgress(
    watchedSeconds,
    Completed);
            var totalLessons = await _unitOfWork.Lessons
    .Query()
    .CountAsync(
        x => x.Section.CourseId ==
             lesson.Section.CourseId,
        cancellationToken);
            var completedLessons = await _unitOfWork.LessonsProgress
    .Query()
    .CountAsync(
        x => x.UserId ==
             _currentUserService.UserId &&
             x.IsCompleted &&
             x.Lesson.Section.CourseId ==
             lesson.Section.CourseId,
        cancellationToken);
            decimal percent =
    totalLessons == 0
        ? 0
        : (decimal)completedLessons /
          totalLessons * 100;


            if (enrollment == null)
            {
                throw new InvalidOperationException(
                    "Enrollment not found.");
            }
            enrollment.UpdateProgress(percent);

            await _unitOfWork.SaveChangesAsync(
    cancellationToken);


        }

    }
}
