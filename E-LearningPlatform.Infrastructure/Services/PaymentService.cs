using AutoMapper;
using E_LearningPlatform.Application.DTOs.Payment;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService (
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IEnrollmentService enrollmentService,
            ILogger<PaymentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        public async Task<PaymentResponseDto> CreatePaymentAsync (
            int orderId,
            PaymentProvider provider,
            CancellationToken cancellationToken = default)
        {
            var userId =
                _currentUserService.UserId
                ?? throw new UnauthorizedAccessException();

            var order = await _unitOfWork.Orders
                .GetByIdAsync(orderId, cancellationToken);

            if (order == null)
                throw new NotFoundException("Order not found.");

            if (order.UserId != userId)
                throw new UnauthorizedAccessException();

            if (order.Status != OrderStatus.Pending)
                throw new BadRequestException(
                    "Only pending orders can be paid.");

            var hasPendingPayment =
                await _unitOfWork.Payments
                .Query()
                .AnyAsync(
                    x =>
                        x.OrderId == orderId
                        &&
                        x.Status == PaymentStatus.Pending,
                    cancellationToken);

            if (hasPendingPayment)
            {
                throw new BadRequestException(
                    "This order already has a pending payment.");
            }

            var hasPaidPayment =
                await _unitOfWork.Payments
                .Query()
                .AnyAsync(
                    x =>
                        x.OrderId == orderId
                        &&
                        x.Status == PaymentStatus.Paid,
                    cancellationToken);

            if (hasPaidPayment)
            {
                throw new BadRequestException(
                    "Order is already paid.");
            }

            var payment = new Payment(
                userId,
                order.Id,
                order.Total,
                provider);

            payment.AttachProviderReference(
                Guid.NewGuid().ToString());

            await _unitOfWork.Payments
                .AddAsync(payment, cancellationToken);

            await _unitOfWork
                .SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Payment {PaymentId} created for order {OrderId}",
                payment.Id,
                order.Id);

            return _mapper.Map<PaymentResponseDto>(payment);
        }

        public async Task ConfirmPaymentAsync (
            int paymentId,
            string providerChargeId,
            string receiptUrl,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(providerChargeId))
                throw new BadRequestException(
                    "Provider charge id is required.");

            if (string.IsNullOrWhiteSpace(receiptUrl))
                throw new BadRequestException(
                    "Receipt url is required.");

            var payment = await _unitOfWork.Payments
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == paymentId,
                    cancellationToken);

            if (payment == null)
                throw new NotFoundException(
                    "Payment not found.");

            if (payment.Status != PaymentStatus.Pending)
                throw new BadRequestException(
                    "Only pending payments can be confirmed.");

            var order = await _unitOfWork.Orders
                .Query()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(
                    x => x.Id == payment.OrderId,
                    cancellationToken);

            if (order == null)
                throw new NotFoundException(
                    "Order not found.");

            payment.MarkPaid(
                providerChargeId,
                receiptUrl);

            order.MarkPaid();

            payment.AddDomainEvent(
                new AuditEvent(
                    _currentUserService.UserId,
                    "PaymentCompleted",
                    nameof(Payment),
                    payment.Id,
                    null,
                    payment));

            payment.AddDomainEvent(
                new PaymentCompletedEvent(
                    payment.Id,
                    order.Id,
                    payment.UserId));

            await _unitOfWork
                .SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Payment {PaymentId} confirmed successfully",
                payment.Id);
        }

        public async Task FailPaymentAsync (
            int paymentId,
            CancellationToken cancellationToken = default)
        {
            var payment = await _unitOfWork.Payments
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == paymentId,
                    cancellationToken);

            if (payment == null)
                throw new NotFoundException(
                    "Payment not found.");

            if (payment.Status != PaymentStatus.Pending)
                throw new BadRequestException(
                    "Only pending payments can fail.");

            payment.MarkFailed();

            await _unitOfWork
                .SaveChangesAsync(cancellationToken);

            _logger.LogWarning(
                "Payment {PaymentId} marked as failed",
                payment.Id);
        }
    }
}