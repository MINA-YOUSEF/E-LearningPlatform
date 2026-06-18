using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using MediatR;


namespace E_LearningPlatform.Infrastructure.Services
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        public DomainEventDispatcher (IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task DispatchAsync (IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

        }
    }
}
