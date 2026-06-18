using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Wrapper;
using MediatR;


namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    public class RefundRejectedEventHandler
: INotificationHandler<DomainEventNotification<RefundRejectedEvent>>
    {
        private readonly INotificationService _notification;

        public RefundRejectedEventHandler (
            INotificationService notification)
        {
            _notification = notification;
        }

        public async Task Handle (
            DomainEventNotification<RefundRejectedEvent> notification,
            CancellationToken cancellationToken)
        {
            await _notification.CreateAsync(

                notification.Notification.UserId,

                "Refund Rejected",

                $"Reason : {notification.Notification.Reason}",

                NotificationType.RefundRejected,

                cancellationToken);
        }
    }
}
