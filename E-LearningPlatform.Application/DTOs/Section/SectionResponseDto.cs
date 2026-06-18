namespace E_LearningPlatform.Application.DTOs.Section
{
    public class SectionResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int CourseId { get; set; }

        public int Order { get; set; }

        public bool IsLocked { get; set; }

        public int LessonsCount { get; set; }

        public int? TotalDurationInMinutes { get; set; }
    }
}
