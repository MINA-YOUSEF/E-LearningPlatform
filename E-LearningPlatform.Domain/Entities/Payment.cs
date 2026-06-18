using System;
using System.Collections.Generic;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class Payment : BaseEntity
    {
        private readonly List<PaymentItem> _items = new();
        private Payment () { }

        public Payment (
     int userId,
     int orderId,
     Money amount,
     PaymentProvider provider)
        {
            UserId = userId;
            OrderId = orderId;
            Amount = amount;
            Provider = provider;

            Status = PaymentStatus.Pending;
        }
        public int UserId { get; private set; }
        public Money Amount { get; private set; }
        public Money? TaxAmount { get; private set; }
        public string? ProviderChargeId { get; private set; }
        public string? ReceiptUrl { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public PaymentStatus Status { get; private set; }
        public PaymentProvider Provider { get; private set; }
        public DateTime? RefundedAt { get; private set; }
        public int? OrderId { get; private set; }
        public Order? Order { get; private set; }
        public DateTime? PaidAtUtc { get; private set; }
        public void MarkPaid (string providerChargeId, string receiptUrl)
        {
            if (Status == PaymentStatus.Paid)
                throw new InvalidOperationException(
                    "Payment already confirmed.");

            ProviderChargeId = providerChargeId;

            ReceiptUrl = receiptUrl;

            Status = PaymentStatus.Paid;

            PaidAtUtc = DateTime.UtcNow;
        }
        public void MarkFailed ()
        {
            if (Status == PaymentStatus.Paid)
            {
                throw new InvalidOperationException(
                    "Paid payment cannot be marked as failed.");
            }

            Status = PaymentStatus.Failed;
        }
        public void MarkRefunded ()
        {
            Status = PaymentStatus.Refunded;
            RefundedAt = DateTime.UtcNow;
        }
        public void Cancel ()
        {
            Status = PaymentStatus.Cancelled;
        }
        public void AttachProviderReference (string providerChargeId)
        {
            ProviderChargeId = providerChargeId;
        }
    }
}
