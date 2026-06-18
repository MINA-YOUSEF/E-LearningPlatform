using E_LearningPlatform.Domain.DomainEvent;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync (
        IEnumerable<IDomainEvent> domainEvents);
    }
}
