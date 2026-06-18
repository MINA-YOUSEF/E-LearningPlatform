namespace E_LearningPlatform.Application.DTOs.Enrollment
{
    public class CourseSummaryDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ThumbnailUrl { get; set; } = null!;

        public string Slug { get; set; } = null!;
    }
}