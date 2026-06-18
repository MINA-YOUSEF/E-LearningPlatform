using E_LearningPlatform.Application.DTOs.Progress;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IProgressService 
    {
        Task MarkLessonProgressAsync(
            int lessonId,
            int watchedSeconds, 
            bool Completed ,
            CancellationToken cancellationToken=default);
        Task<LessonProgressResponseDto> GetLessonProgressAsync(
            int lessonId, 
            CancellationToken cancellationToken = default);
        Task<CourseProgressResponseDto> GetCourseProgressAsync(int courseId, CancellationToken cancellationToken = default);    
    }
}
