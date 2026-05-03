using System;
using System.Collections.Generic;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class Payment : BaseEntity
    {
        private readonly List<PaymentItem> _items = new();
        private Payment() { }

        public Payment(int userId, Money amount, PaymentProvider provider)
        {
            UserId = userId;
            Amount = amount;
            Provider = provider;
            PaidAtUtc = DateTime.UtcNow;
            Status = PaymentStatus.Pending;
        }

        public int UserId { get; private set; }
        public Money Amount { get; private set; }
        public Money? TaxAmount { get; private set; }
        public string? ProviderChargeId { get; private set; }
        public string? ReceiptUrl { get; private set; }
        public DateTime PaidAtUtc { get; private set; }
        public PaymentStatus Status { get; private set; }
        public PaymentProvider Provider { get; private set; }
        public DateTime? RefundedAt { get; private set; }
        public int? OrderId { get; private set; }
        public Order? Order { get; private set; }
        public IReadOnlyCollection<PaymentItem> Items => _items.AsReadOnly();

        public void MarkPaid(string providerChargeId, string receiptUrl)
        {
            ProviderChargeId = providerChargeId;
            ReceiptUrl = receiptUrl;
            Status = PaymentStatus.Paid;
            PaidAtUtc = DateTime.UtcNow;
        }

        public void MarkRefunded()
        {
            Status = PaymentStatus.Refunded;
            RefundedAt = DateTime.UtcNow;
        }
    }
}
