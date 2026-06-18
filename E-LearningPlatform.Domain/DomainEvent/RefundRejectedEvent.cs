using E_LearningPlatform.Domain.DomainEvent;

public sealed class RefundRejectedEvent
    : BaseDomainEvent
{
    public RefundRejectedEvent (
        int userId,
        string reason)
    {
        UserId = userId;

        Reason = reason;
    }

    public int UserId { get; }

    public string Reason { get; }
}