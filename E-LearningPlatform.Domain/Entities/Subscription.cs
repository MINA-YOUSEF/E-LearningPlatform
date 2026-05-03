using System;
using E_LearningPlatform.Domain.Enums;

namespace E_LearningPlatform.Domain.Entities
{
    public class Subscription : BaseEntity
    {
        private Subscription() { }

        public Subscription(int userId, DateTime start, DateTime end)
        {
            UserId = userId;
            StartAtUtc = start;
            EndAtUtc = end;
            Status = SubscriptionStatus.Active;
        }

        public int UserId { get; private set; }
        public DateTime StartAtUtc { get; private set; }
        public DateTime EndAtUtc { get; private set; }
        public SubscriptionStatus Status { get; private set; }
    }
}
