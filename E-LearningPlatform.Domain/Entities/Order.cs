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

        public Order(int userId)
        {
            UserId = userId;
            Status = OrderStatus.Pending;
        }

        public int UserId { get; private set; }
        public Money Total { get; private set; }
        public Money Tax { get; private set; }
        public OrderStatus Status { get; private set; }
        public int? PaymentId { get; private set; }
        public Payment? Payment { get; private set; }
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public void MarkPaid()
        {
            if (Status == OrderStatus.Paid)
            {
                throw new InvalidOperationException(
                    "Order already paid.");
            }

            Status = OrderStatus.Paid;
        }
        public void AddItem(OrderItem item)
        {
            _items.Add(item);
        }
        public void Cancel() => Status = OrderStatus.Cancelled;

        public void CalculateTotal()
        {
            if (!Items.Any()) throw new InvalidOperationException("Order must contain items.") ;
            Money total= _items.First().Price;
            foreach (var item in _items.Skip(1))
            {
                total = total.Add(item.Price);
            }
            Total = total;


        }
    }
}
