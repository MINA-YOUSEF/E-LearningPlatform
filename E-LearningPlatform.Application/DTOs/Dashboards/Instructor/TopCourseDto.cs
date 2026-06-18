namespace E_LearningPlatform.Application.DTOs.Dashboards.Instructor
{
    public class TopCourseDto
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = null!;

        public int EnrollmentCount { get; set; }

        public decimal Revenue { get; set; }

        public decimal Rating { get; set; }
    }
}
