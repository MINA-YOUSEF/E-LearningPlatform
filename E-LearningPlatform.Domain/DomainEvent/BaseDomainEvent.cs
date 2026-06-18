using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.DomainEvent
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }
}
