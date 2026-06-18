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
    public class ReviewAddedNotificationHandler : INotificationHandler<DomainEventNotification<ReviewAddedEvent>>
    {
        private readonly INotificationService
        _notificationService;

        public ReviewAddedNotificationHandler (
            INotificationService notificationService)
        {
            _notificationService =
                notificationService;
        }

        public async Task Handle (
           DomainEventNotification<ReviewAddedEvent> notification,
            CancellationToken cancellationToken)
        {
            await _notificationService.CreateAsync(
                notification.Notification.InstructorId,
                "New Review",
                $"You received a new {notification.Notification.Rating}-star review on {notification.Notification.CourseTitle}.",
                NotificationType.Review,
                cancellationToken);
        }


    }
}
