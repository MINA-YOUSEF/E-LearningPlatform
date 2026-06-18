using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.DomainEvent
{
    public class PaymentCompletedEvent : BaseDomainEvent
    {
        public int PaymentId { get; }
        public int OrderId { get; }
        public int UserId { get; }
        public PaymentCompletedEvent (int paymentId, int orderId, int userId)
        {
            PaymentId = paymentId;
            OrderId = orderId;
            UserId = userId;
        }


    }
}
