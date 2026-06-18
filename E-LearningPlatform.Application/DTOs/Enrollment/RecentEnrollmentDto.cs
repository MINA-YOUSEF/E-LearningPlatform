namespace E_LearningPlatform.Application.DTOs.Enrollment
{
    public class RecentEnrollmentDto
    {
        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = null!;

        public string CourseSlug { get; set; } = null!;

        public string? ThumbnailUrl { get; set; }

        public DateTime EnrolledAt { get; set; }
    }
}