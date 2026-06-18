namespace E_LearningPlatform.Application.DTOs.Progress
{
    public class CourseProgressResponseDto
        {
            public int CourseId { get; set; }

            public string CourseTitle { get; set; } = null!;

            public decimal ProgressPercent { get; set; }

            public DateTime? LastAccessedAt { get; set; }
        }
    
}