using E_LearningPlatform.Application.DTOs.Lesson;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ILessonService
    {
        public Task<LessonCreatedResponseDto> AddLessonAsync(AddLessonRequestDto request);
        public Task<LessonCreatedResponseDto> UpdateLessonAsync(int lessonId, AddLessonRequestDto request);
        public Task DeleteLessonAsync(int lessonId, int sectionId);
        public Task ReorderLessonsAsync(int sectionId, List<int> orderedLessonIds);
    }
}
