using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Notification;
using E_LearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task CreateAsync (int userId,
            string title,
            string message,
            NotificationType type,
            CancellationToken cancellationToken = default);
        Task<PagedResult<NotificationResponseDto>> GetUserNotificationsAsync (PagedRequest request,
            CancellationToken cancellationToken = default);
        Task<int> GetUnreadCountAsync (
    CancellationToken cancellationToken = default);
        Task MarkAsReadAsync (int notificationId, CancellationToken cancellationToken = default);
        Task MarkAllAsReadAsync (
    CancellationToken cancellationToken = default);
    }
}
