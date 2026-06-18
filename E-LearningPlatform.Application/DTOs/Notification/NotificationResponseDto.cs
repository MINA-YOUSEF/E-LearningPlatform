using E_LearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Notification
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }

        public int RecipientUserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public string? ReferenceId { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
