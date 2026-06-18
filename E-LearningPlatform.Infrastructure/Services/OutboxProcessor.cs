using E_LearningPlatform.Application.Interfaces.Outbox;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace E_LearningPlatform.Infrastructure.Services
{
    public class OutboxProcessor : IOutboxProcessor
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public OutboxProcessor (AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task ProcessAsync ()
        {
            var messages = await _context.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null).Take(50)
                .ToListAsync();
            foreach (var message in messages)
            {
                try
                {
                    switch (message.Type)
                    {
                        case nameof(CourseCompletedEvent):
                            var courseCompletedEvent =
                                JsonSerializer.Deserialize<CourseCompletedEvent>(message.Content);
                            await _mediator.Publish(courseCompletedEvent!);
                            break;
                        case nameof(CertificateGeneratedEvent):
                            var certificateGeneratedEvent =
                                JsonSerializer.Deserialize<CertificateGeneratedEvent>(message.Content);
                            await _mediator.Publish(certificateGeneratedEvent!);
                            break;
                        case nameof(PaymentCompletedEvent):
                            var paymentCompletedEvent =
                                JsonSerializer.Deserialize<PaymentCompletedEvent>(message.Content);
                            await _mediator.Publish(paymentCompletedEvent!);
                            break;
                        case nameof(ReviewAddedEvent):
                            var reviewAddedEvent =
                            JsonSerializer.Deserialize<ReviewAddedEvent>(message.Content);
                            await _mediator.Publish(reviewAddedEvent!);
                            break;
                    }
                    message.MarkProcessed();
                }
                catch (Exception ex)
                {
                    message.MarkFailed(
                      ex.ToString());

                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
