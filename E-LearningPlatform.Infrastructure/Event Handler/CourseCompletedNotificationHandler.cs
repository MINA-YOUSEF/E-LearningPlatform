using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    public class CourseCompletedNotificationHandler : INotificationHandler<DomainEventNotification<CourseCompletedEvent>>
    {
        private readonly INotificationService _notificationService;
        public CourseCompletedNotificationHandler (INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle (DomainEventNotification<CourseCompletedEvent> notification, CancellationToken cancellationToken)
        {
            await _notificationService.CreateAsync(
                userId: notification.Notification.UserId,
                title: "Congratulations on Completing the Course!",
                message: $"You have successfully completed the course: {notification.Notification.CourseTitle}. Keep up the great work!",
                NotificationType.CourseCompletion

                );
        }
    }
}
