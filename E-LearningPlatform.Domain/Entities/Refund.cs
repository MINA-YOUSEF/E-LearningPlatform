using System;

namespace E_LearningPlatform.Domain.Entities
{
    public class Refund : BaseEntity
    {
        private Refund() { }

        public Refund(int orderId, int paymentId, int userId, decimal amount)
        {
            OrderId = orderId;
            PaymentId = paymentId;
            UserId = userId;
            Amount = amount;
            RequestedAtUtc = DateTime.UtcNow;
        }

        public int OrderId { get; private set; }
        public Order Order { get; private set; }
        public int PaymentId { get; private set; }
        public Payment Payment { get; private set; }
        public int UserId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime RequestedAtUtc { get; private set; }
        public DateTime? ProcessedAtUtc { get; private set; }
        public bool Approved { get; private set; }
    }
}
