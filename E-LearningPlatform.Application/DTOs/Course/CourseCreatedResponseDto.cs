namespace E_LearningPlatform.Application.DTOs.Course
{
    public class CourseCreatedResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public decimal Price { get; set; }
        public string Currency { get; set; }

        public string Language { get; set; }

        public bool IsPublished { get; set; }

        public bool IsActive { get; set; }

        public string ApprovalStatus { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}


