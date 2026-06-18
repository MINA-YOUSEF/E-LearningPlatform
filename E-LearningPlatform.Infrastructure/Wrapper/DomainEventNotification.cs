using E_LearningPlatform.Domain.DomainEvent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Wrapper
{
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : IDomainEvent
    {
        public TDomainEvent Notification { get; }

        public DomainEventNotification (TDomainEvent notification)
        {

            Notification = notification;

        }
    }
}
