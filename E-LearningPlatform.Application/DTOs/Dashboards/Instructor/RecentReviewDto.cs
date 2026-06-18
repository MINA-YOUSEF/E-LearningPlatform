namespace E_LearningPlatform.Application.DTOs.Dashboards.Instructor
{
    public class RecentReviewDto
    {
        public string CourseTitle { get; set; } = null!;

        public int Rating { get; set; }

        public string Comment { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; }
    }
}
