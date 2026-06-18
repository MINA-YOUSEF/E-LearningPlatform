namespace E_LearningPlatform.Infrastructure.Services
{
    using AutoMapper;
    using E_LearningPlatform.Application.DTOs.Refund;
    using E_LearningPlatform.Application.Interfaces.Repositories;
    using E_LearningPlatform.Application.Interfaces.Services;
    using E_LearningPlatform.Domain.Enums;
    using Microsoft.EntityFrameworkCore;

    public class RefundService : IRefundService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentUserService _currentUserService;

        private readonly IMapper _mapper;

        private readonly IAuditService _auditService;

        public RefundService (
     IUnitOfWork unitOfWork,
     ICurrentUserService currentUserService,
     IMapper mapper,
     IAuditService auditService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _auditService = auditService;
        }

        public async Task<RefundResponseDto> RequestRefundAsync (
            CreateRefundRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var payment = await _unitOfWork.Payments
                .Query()
                .Include(x => x.Order)
                .FirstOrDefaultAsync(
                    x => x.Id == request.PaymentId,
                    cancellationToken);

            if (payment == null)
            {
                throw new InvalidOperationException(
                    "Payment not found.");
            }

            if (payment.UserId != _currentUserService.UserId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot refund this payment.");
            }

            if (payment.Status != PaymentStatus.Paid)
            {
                throw new InvalidOperationException(
                    "Only paid payments can be refunded.");
            }

            // Refund window = 14 days
            if (payment.PaidAtUtc < DateTime.UtcNow.AddDays(-14))
            {
                throw new InvalidOperationException(
                    "Refund period expired.");
            }

            var alreadyRequested =
                await _unitOfWork.Refunds
                .Query()
                .AnyAsync(
                    x =>
                        x.PaymentId == payment.Id
                        &&
                        x.Status != RefundStatus.Rejected,
                    cancellationToken);

            if (alreadyRequested)
            {
                throw new InvalidOperationException(
                    "Refund already requested.");
            }

            var refund = new Refund(
                payment.OrderId!.Value,
                payment.Id,
                payment.UserId,
                payment.Amount.Amount,
                request.Reason);

            await _unitOfWork.Refunds
                .AddAsync(refund, cancellationToken);
            await _auditService.LogAsync(
    "Create Refund Request",
    nameof(Refund),
    refund.Id,

    null,

    new
    {
        refund.PaymentId,
        refund.Amount,
        refund.Reason
    },

    cancellationToken);
            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return _mapper.Map<RefundResponseDto>(
                refund);
        }

        public async Task ApproveRefundAsync (
            int refundId,
            CancellationToken cancellationToken = default)
        {
            var refund =
                await _unitOfWork.Refunds
                .Query()
                .Include(x => x.Payment)
                    .ThenInclude(x => x.Order)
                        .ThenInclude(x => x.Items)
                .FirstOrDefaultAsync(
                    x => x.Id == refundId,
                    cancellationToken);

            if (refund == null)
            {
                throw new InvalidOperationException(
                    "Refund not found.");
            }

            if (refund.Status != RefundStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Refund already processed.");
            }

            refund.Approve();

            refund.Payment.MarkRefunded();
            await _auditService.LogAsync(
    "Approve Refund",
    nameof(Refund),
    refund.Id,

    new
    {
        Status = RefundStatus.Pending
    },

    new
    {
        Status = RefundStatus.Approved,
        Amount = refund.Amount
    },

    cancellationToken);

            var courseIds = refund.Payment.Order.Items
     .Select(x => x.CourseId)
     .ToList();

            var enrollments = await _unitOfWork.Enrollments
                .Query()
                .Where(x =>
                    x.UserId == refund.UserId &&
                    courseIds.Contains(x.CourseId))
                .ToListAsync(cancellationToken);

            foreach (var enrollment in enrollments)
            {
                enrollment.Deactivate();
            }

            refund.AddDomainEvent(
                new RefundApprovedEvent(
                    refund.UserId,
                    refund.Amount));

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }

        public async Task RejectRefundAsync (
            int refundId,
            string note,
            CancellationToken cancellationToken = default)
        {
            var refund =
                await _unitOfWork.Refunds
                .GetByIdAsync(
                    refundId,
                    cancellationToken);

            if (refund == null)
            {
                throw new InvalidOperationException(
                    "Refund not found.");
            }

            if (refund.Status != RefundStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Refund already processed.");
            }

            refund.Reject(note);
            await _auditService.LogAsync(
    "Reject Refund",
    nameof(Refund),
    refund.Id,

    new
    {
        Status = RefundStatus.Pending
    },

    new
    {
        Status = RefundStatus.Rejected,
        Note = note
    },

    cancellationToken);

            refund.AddDomainEvent(
                new RefundRejectedEvent(
                    refund.UserId,
                    note));

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }

        public async Task<List<RefundResponseDto>>
            GetPendingRefundsAsync (
            CancellationToken cancellationToken = default)
        {
            var refunds =
                await _unitOfWork.Refunds
                .Query()
                .Include(x => x.Payment)
                .Where(
                    x =>
                        x.Status ==
                        RefundStatus.Pending)
                .OrderByDescending(
                    x => x.RequestedAtUtc)
                .ToListAsync(
                    cancellationToken);

            return _mapper.Map<
                List<RefundResponseDto>>(
                    refunds);
        }
    }
}
