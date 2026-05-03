using System;
using E_LearningPlatform.Domain.Enums;

namespace E_LearningPlatform.Domain.Entities
{
    public class Notification : BaseEntity
    {
        private Notification() { }

        public Notification(int recipientUserId, string title, string message, NotificationType type)
        {
            RecipientUserId = recipientUserId;
            Title = title;
            Message = message;
            Type = type;
        }

        public int RecipientUserId { get; private set; }
        public string Title { get; private set; } = null!;
        public string Message { get; private set; } = null!;
        public NotificationType Type { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime? ReadAt { get; private set; }
        public DateTime? DeliveredAt { get; private set; }
        public string? ReferenceId { get; private set; }

        public void MarkDelivered() => DeliveredAt = DateTime.UtcNow;
        public void MarkRead()
        {
            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }
    }
}
