namespace E_LearningPlatform.Application.DTOs.Section
{
    public class AddSectionRequestDto
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }
    }
}
