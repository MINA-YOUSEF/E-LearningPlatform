using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Wrapper;
using MediatR;


namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    public class RefundApprovedEventHandler
: INotificationHandler<DomainEventNotification<RefundApprovedEvent>>
    {
        private readonly INotificationService _notification;

        public RefundApprovedEventHandler (
            INotificationService notification)
        {
            _notification = notification;
        }

        public async Task Handle (
           DomainEventNotification<RefundApprovedEvent> notification,
            CancellationToken cancellationToken)
        {
            await _notification.CreateAsync(

                notification.Notification.UserId,

                "Refund Approved",

                $"Your refund request for {notification.Notification.Amount}$ has been approved.",

                NotificationType.RefundApproved,

                cancellationToken);
        }
    }
}
