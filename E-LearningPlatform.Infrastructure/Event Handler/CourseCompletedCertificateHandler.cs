using E_LearningPlatform.Application.Interfaces.Jobs;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Infrastructure.Wrapper;
using Hangfire;
using MediatR;

namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    public class CourseCompletedCertificateHandler : INotificationHandler<DomainEventNotification<CourseCompletedEvent>>
    {
        private readonly ICertificateService _certificateService;
        public CourseCompletedCertificateHandler (ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        public async Task Handle (DomainEventNotification<CourseCompletedEvent> notification, CancellationToken cancellationToken)
        {
            BackgroundJob.
                Enqueue<ICertificateGeneratedJob>
                (x =>
                x.CertificateGeneratedAsync(notification.Notification.CourseId, cancellationToken));
        }
    }
}
