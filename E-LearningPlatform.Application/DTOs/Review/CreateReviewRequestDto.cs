namespace E_LearningPlatform.Application.DTOs.Review
{
    public class CreateReviewRequestDto
    {
        public int CourseId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = null!;

        public string? Title { get; set; }
    }
}
