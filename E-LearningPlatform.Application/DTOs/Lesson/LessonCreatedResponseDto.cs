using E_LearningPlatform.Domain.Enums;

namespace E_LearningPlatform.Application.DTOs.Lesson
{
    public class LessonCreatedResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public int SectionId { get; set; }

        public int Order { get; set; }

        public LessonContentType ContentType { get; set; }

        public int DurationInMinutes { get; set; }

        public bool IsPreview { get; set; }

        public string? VideoUrl { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}
