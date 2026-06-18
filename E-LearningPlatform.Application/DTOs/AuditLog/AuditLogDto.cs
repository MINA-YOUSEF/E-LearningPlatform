using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.AuditLog
{
    public class AuditLogDto
    {
        public string Action { get; set; } = null!;
        public string EntityName { get; set; } = null!;
        public int EntityId { get; set; }
        public DateTime OccurredOnUtc { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }

    }
}
