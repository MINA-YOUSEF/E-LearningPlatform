using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace E_LearningPlatform.Domain.Entities
{
    public class OutboxMessage : BaseEntity
    {
        private OutboxMessage () { }

        public OutboxMessage (
            string type,
            string content)
        {
            Type = type;
            Content = content;
            OccurredOnUtc = DateTime.UtcNow;
        }

        public string Type { get; private set; } = null!;

        public string Content { get; private set; } = null!;

        public DateTime OccurredOnUtc { get; private set; }

        public bool IsProcessed { get; private set; }
        public DateTime? ProcessedOnUtc
        { get; private set; }

        public string? Error { get; private set; }

        public void MarkProcessed ()
        {
            ProcessedOnUtc = DateTime.UtcNow;   
            IsProcessed = true;
        }

        public void MarkFailed (string error)
        {
            Error = error;
        }
    }
}
