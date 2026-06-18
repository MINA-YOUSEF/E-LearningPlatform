namespace E_LearningPlatform.Application.DTOs.Progress
{
    public class LessonProgressResponseDto
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = null!;

        public int UserId { get; set; }

        public bool IsCompleted { get; set; }

        public int WatchedSeconds { get; set; }
    }
    
}