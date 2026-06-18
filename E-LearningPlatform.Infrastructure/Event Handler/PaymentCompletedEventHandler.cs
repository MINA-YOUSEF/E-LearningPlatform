using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    public class PaymentCompletedEventHandler : INotificationHandler<DomainEventNotification<PaymentCompletedEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnrollmentService _enrollmentService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<PaymentCompletedEventHandler> _logger;
        public PaymentCompletedEventHandler (IUnitOfWork unitOfWork,
            IEnrollmentService enrollmentService,
            INotificationService notificationService,
            ILogger<PaymentCompletedEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _enrollmentService = enrollmentService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle (DomainEventNotification<PaymentCompletedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
    "Processing PaymentCompletedEvent for order {OrderId}",
    notification.Notification.OrderId);
            var order = await _unitOfWork.Orders.Query()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == notification.Notification.OrderId,
                cancellationToken);
            if (order == null)
                return;
            foreach (var item in order.Items)
            {
                var alreadyEnrolled = await _unitOfWork.Enrollments
                    .Query()
                    .AnyAsync(
                    x => x.UserId == notification.Notification.UserId &&
                    x.CourseId == item.CourseId,
                    cancellationToken);
                if (alreadyEnrolled)
                {
                    continue;
                }
                await _enrollmentService.EnrollAsync(notification.Notification.UserId, item.CourseId, item.Price, cancellationToken);
                _logger.LogInformation(
         "User {UserId} enrolled successfully after payment",
         notification.Notification.UserId);
            }
            await _notificationService.CreateAsync(notification.Notification.UserId, "Payment Completed", $"Your payment for order {order.Id} has been completed and you have been enrolled in the courses.",
               NotificationType.Payment, cancellationToken);
        }
    }
}
