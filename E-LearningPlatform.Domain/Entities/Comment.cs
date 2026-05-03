using System;

namespace E_LearningPlatform.Domain.Entities
{
    public class Comment : BaseEntity
    {
        private Comment() { }

        public Comment(int userId, int lessonId, string content)
        {
            UserId = userId;
            LessonId = lessonId;
            Content = content;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public int UserId { get; private set; }
        public int LessonId { get; private set; }
        public Lesson Lesson { get; private set; } = null!;
        public string Content { get; private set; } = null!;
    }
}
