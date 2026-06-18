using E_LearningPlatform.Application.DTOs.Course;

namespace E_LearningPlatform.Application.DTOs.Enrollment
{
    public class EnrollmentCourseResponseDto
    {
        public DateTime EnrolledAt { get; set; }

        public decimal PriceAmount { get; set; }

        public string Currency { get; set; } = null!;

        public string Status { get; set; } = null!;

        public bool IsCompleted { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public int CourseId { get; set; }

        public decimal ProgressPercent { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public DateTime? LastAccessedAt { get; set; }

        public CourseSummaryDto Course { get; set; } = null!;
    }
}