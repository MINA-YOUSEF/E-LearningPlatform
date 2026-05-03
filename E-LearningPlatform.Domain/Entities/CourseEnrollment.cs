using System;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class CourseEnrollment : BaseEntity
    {
        private CourseEnrollment() { }

        public CourseEnrollment(int courseId, int userId, Money price)
        {
            CourseId = courseId;
            UserId = userId;
            Price = price;
            Status = EnrollmentStatus.Active;
            EnrolledAt = DateTime.UtcNow;
        }

        public DateTime EnrolledAt { get; private set; }
        public Money Price { get; private set; }
        public EnrollmentStatus Status { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsActive { get; private set; } = true;
        public int UserId { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public decimal ProgressPercent { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public DateTime? LastAccessedAt { get; private set; }

        public void UpdateProgress(decimal percent)
        {
            if (percent < 0 || percent > 100) throw new ArgumentOutOfRangeException(nameof(percent));
            ProgressPercent = percent;
            if (percent >= 100)
            {
                IsCompleted = true;
                Status = EnrollmentStatus.Completed;
            }
            LastAccessedAt = DateTime.UtcNow;
        }
    }
}
