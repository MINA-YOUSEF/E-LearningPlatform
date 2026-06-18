using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Infrastructure.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    public class AuditEventHandler : INotificationHandler<DomainEventNotification<AuditEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditService _auditService;
        public AuditEventHandler (IUnitOfWork unitOfWork, IAuditService auditService)
        {
            _unitOfWork = unitOfWork;
            _auditService = auditService;
        }
        public async Task Handle (DomainEventNotification<AuditEvent> notification, CancellationToken cancellationToken)
        {
            await _auditService.LogAsync(
                notification.Notification.Action,
                notification.Notification.EntityName,
                notification.Notification.EntityId,
                notification.Notification.OldValues,
                notification.Notification.NewValues,
                cancellationToken
            );
        }
    }
}
