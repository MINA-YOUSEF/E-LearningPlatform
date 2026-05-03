using System;

namespace E_LearningPlatform.Domain.Entities
{
    public class CourseProgress : BaseEntity
    {
        private CourseProgress() { }

        public CourseProgress(int courseId, int userId)
        {
            CourseId = courseId;
            UserId = userId;
            ProgressPercent = 0;
        }

        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public int UserId { get; private set; }
        public decimal ProgressPercent { get; private set; }
        public DateTime? LastAccessedAt { get; private set; }

        public void UpdateProgress(decimal percent)
        {
            if (percent < 0 || percent > 100) throw new ArgumentOutOfRangeException(nameof(percent));
            ProgressPercent = percent;
            LastAccessedAt = DateTime.UtcNow;
        }
    }
}
