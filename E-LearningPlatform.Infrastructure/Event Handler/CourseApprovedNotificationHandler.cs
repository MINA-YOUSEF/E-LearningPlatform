using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Wrapper;
using MediatR;


namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    class CourseApprovedNotificationHandler
   : INotificationHandler<
       DomainEventNotification<
           CourseApprovedEvent>>
    {
        private readonly
            INotificationService
            _notificationService;
        public CourseApprovedNotificationHandler (INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle (
          DomainEventNotification<
              CourseApprovedEvent> notification,
          CancellationToken cancellationToken)
        {
            await _notificationService
                .CreateAsync(
                    notification.Notification
                        .InstructorId,

                    "Course Approved",

                    $"Your course '{notification.Notification.CourseTitle}' has been approved.",

                    NotificationType.CourseApproved,
                    cancellationToken);

        }
    }

    public class CourseRejectedNotificationHandler :
        INotificationHandler<DomainEventNotification<CourseRejectedEvent>>
    {
        private readonly
            INotificationService
            _notificationService;
        public CourseRejectedNotificationHandler (INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle (DomainEventNotification<CourseRejectedEvent> notification, CancellationToken cancellationToken)
        {
            await _notificationService
                .CreateAsync(
                   notification.Notification.InstructorId,

                    "Course Rejected",

                    $"Your course '{notification.Notification.CourseTitle}' was rejected. Reason: {notification.Notification.RejectionReason}",

                    NotificationType.CourseRejected,
                    cancellationToken);
        }
    }


}
