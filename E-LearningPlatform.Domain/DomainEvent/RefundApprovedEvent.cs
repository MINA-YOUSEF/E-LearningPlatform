using E_LearningPlatform.Domain.DomainEvent;

public sealed class RefundApprovedEvent
    : BaseDomainEvent
{
    public RefundApprovedEvent (
        int userId,
        decimal amount)
    {
        UserId = userId;

        Amount = amount;
    }

    public int UserId { get; }

    public decimal Amount { get; }
}
