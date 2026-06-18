using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;

public class Refund : BaseEntity
{
    private Refund () { }

    public Refund (
        int orderId,
        int paymentId,
        int userId,
        decimal amount,
        string reason)
    {
        OrderId = orderId;
        PaymentId = paymentId;
        UserId = userId;
        Amount = amount;
        Reason = reason;
        RequestedAtUtc = DateTime.UtcNow;
        Status = RefundStatus.Pending;
    }

    public int OrderId { get; private set; }

    public Order Order { get; private set; } = null!;

    public int PaymentId { get; private set; }

    public Payment Payment { get; private set; } = null!;

    public int UserId { get; private set; }

    public decimal Amount { get; private set; }

    public string Reason { get; private set; } = null!;

    public DateTime RequestedAtUtc { get; private set; }

    public DateTime? ProcessedAtUtc { get; private set; }

    public RefundStatus Status { get; private set; }

    public string? AdminNote { get; private set; }

    public void Approve (string? adminNote = null)
    {
        Status = RefundStatus.Approved;
        ProcessedAtUtc = DateTime.UtcNow;
        AdminNote = adminNote;
    }

    public void Reject (string? adminNote = null)
    {
        Status = RefundStatus.Rejected;
        ProcessedAtUtc = DateTime.UtcNow;
        AdminNote = adminNote;
    }
   
}