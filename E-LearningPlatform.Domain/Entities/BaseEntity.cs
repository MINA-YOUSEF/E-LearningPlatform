using System;

namespace E_LearningPlatform.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public int? CreatedByUserId { get; protected set; }
        public int? UpdatedByUserId { get; protected set; }

        protected BaseEntity()
        {
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void MarkDeleted(int? userId = null)
        {
            IsDeleted = true;
            UpdatedAtUtc = DateTime.UtcNow;
            UpdatedByUserId = userId ?? UpdatedByUserId;
        }

        public void Touch(int? userId = null)
        {
            UpdatedAtUtc = DateTime.UtcNow;
            UpdatedByUserId = userId ?? UpdatedByUserId;
        }
    }
}
