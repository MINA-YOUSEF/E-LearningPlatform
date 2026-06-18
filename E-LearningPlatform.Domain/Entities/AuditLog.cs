using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        private AuditLog () { }
        public AuditLog (
            int? userId,
            string action,
            string entityName,
            int entityId,
            string? oldValues,
            string? newValues)
        {
            UserId = userId;
            Action = action;
            EntityName = entityName;
            EntityId = entityId;
            OldValues = oldValues;
            NewValues = newValues;
            OccurredOnUtc = DateTime.UtcNow;
        }
        public int? UserId { get; private set; }
        public string Action { get; private set; } = null!;
        public string EntityName { get; private set; } = null!;
        public int EntityId { get; private set; }
        public string? OldValues { get; private set; }
        public string? NewValues { get; private set; }
        public DateTime OccurredOnUtc { get; private set; }
    }
}
