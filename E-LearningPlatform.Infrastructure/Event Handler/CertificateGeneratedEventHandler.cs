using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Jobs;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Wrapper;
using Hangfire;
using MediatR;

namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    public class CertificateGeneratedEventHandler :
        INotificationHandler<DomainEventNotification<CertificateGeneratedEvent>>

    {
        private readonly INotificationService _notificationService;

        public CertificateGeneratedEventHandler (INotificationService notificationService
            )
        {
            _notificationService = notificationService;
        }

        public async Task Handle (DomainEventNotification<CertificateGeneratedEvent> notification, CancellationToken cancellationToken)
        {

            await _notificationService.CreateAsync(
                notification.Notification.UserId,
                "Certificate Generated",
                $"Congratulations! You have earned a certificate for completing the course" +
                $": {notification.Notification.CourseTitle}.",
                NotificationType.CertificateIssued,
                cancellationToken
            );

        }
    }
}
