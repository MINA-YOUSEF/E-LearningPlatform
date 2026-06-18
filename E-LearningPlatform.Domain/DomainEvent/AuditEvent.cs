using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.DomainEvent
{
    public class AuditEvent : BaseDomainEvent
    {
        public int? UserId { get; }

        public string Action { get; }

        public string EntityName { get; }   

        public int EntityId { get; }

        public object? OldValues { get; }

        public object? NewValues { get; }

        public AuditEvent (int? userId, string action, string entityName, int entityId, object? oldValues, object? newValues)
        {
            UserId = userId;
            Action = action;
            EntityName = entityName;
            EntityId = entityId;
            OldValues = oldValues;
            NewValues = newValues;
        }
    }
}
