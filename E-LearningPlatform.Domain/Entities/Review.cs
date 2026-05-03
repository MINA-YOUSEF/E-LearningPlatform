using System;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class Review : BaseEntity
    {
        private Review() { }

        public Review(int courseId, int userId, Rating rating, string comment, string? title = null)
        {
            CourseId = courseId;
            UserId = userId;
            Rating = rating.Value;
            Comment = comment;
            Title = title;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public int UserId { get; private set; }
        public int Rating { get; private set; }
        public string Comment { get; private set; } = null!;
        public string? Title { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public int HelpfulCount { get; private set; }

        public void MarkHelpful() => HelpfulCount++;
    }
}
