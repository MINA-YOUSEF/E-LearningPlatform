using System;
using System.Collections.Generic;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class Order : BaseEntity
    {
        private readonly List<OrderItem> _items = new();
        private Order() { }

        public Order(int userId, Money total, Money tax)
        {
            UserId = userId;
            Total = total;
            Tax = tax;
            Status = OrderStatus.Pending;
        }

        public int UserId { get; private set; }
        public Money Total { get; private set; }
        public Money Tax { get; private set; }
        public OrderStatus Status { get; private set; }
        public int? PaymentId { get; private set; }
        public Payment? Payment { get; private set; }
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public void MarkPaid(int paymentId)
        {
            PaymentId = paymentId;
            Status = OrderStatus.Paid;
        }

        public void Cancel() => Status = OrderStatus.Cancelled;
    }
}
