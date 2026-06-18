using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Wrapper;
using MediatR;

namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    public class MessageSentEventHandler
: INotificationHandler<
    DomainEventNotification<MessageSentEvent>>
    {
        private readonly INotificationService _notification;

        public MessageSentEventHandler (
            INotificationService notification)
        {
            _notification = notification;
        }

        public async Task Handle (
            DomainEventNotification<MessageSentEvent> notification,

            CancellationToken cancellationToken)
        {
            await _notification.CreateAsync(

                notification.Notification.ReceiverId,

                "New Message",

                notification.Notification.Content,

                NotificationType.NewMessage,

                cancellationToken);
        }
    }
}
