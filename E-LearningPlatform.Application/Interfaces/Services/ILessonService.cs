using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Lesson;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ILessonService
    {
        public Task<LessonCreatedResponseDto> AddLessonAsync(AddLessonRequestDto request);
        public Task<LessonCreatedResponseDto> UpdateLessonAsync(int lessonId, AddLessonRequestDto request);
        public Task<LessonDetailsResponseDto> GetLessonByIdAsync(int lessonId);
        public Task<PagedResult<LessonDetailsResponseDto>> GetLessonsBySectionIdAsync(int sectionId, PagedRequest PagedRequest);
        public Task DeleteLessonAsync(int lessonId, int sectionId);
        public Task ReorderLessonsAsync(int sectionId, List<int> orderedLessonIds);
    }
}
