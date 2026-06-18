namespace E_LearningPlatform.Domain.Entities
{
    public class LessonProgress : BaseEntity
    {
        private LessonProgress() { }

        public LessonProgress(int lessonId, int userId)
        {
            LessonId = lessonId;
            UserId = userId;
            IsCompleted = false;
            WatchedSeconds = 0;
        }

        public int LessonId { get; private set; }
        public Lesson Lesson { get; private set; }
        public int UserId { get; private set; }
        public bool IsCompleted { get; private set; }
        public int WatchedSeconds { get; private set; }

        public void MarkProgress(int watchedSeconds, bool completed = false)
        {
            if (watchedSeconds < 0) throw new ArgumentOutOfRangeException(nameof(watchedSeconds));
          
            if (watchedSeconds > WatchedSeconds)
            WatchedSeconds = watchedSeconds;

            IsCompleted = completed || IsCompleted;
            Touch(UserId);
        }
    }
}
