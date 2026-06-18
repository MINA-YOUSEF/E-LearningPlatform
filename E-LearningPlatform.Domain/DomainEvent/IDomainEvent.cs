using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.DomainEvent
{
    public interface IDomainEvent
    {
        DateTime OccurredOnUtc { get; }
    }
}
