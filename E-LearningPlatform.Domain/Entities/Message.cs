using System;

namespace E_LearningPlatform.Domain.Entities
{
    public class Message : BaseEntity
    {
        private Message() { }

        public Message(int conversationId, int senderId, string content)
        {
            ConversationId = conversationId;
            SenderId = senderId;
            Content = content;
            SentAtUtc = DateTime.UtcNow;
        }

        public int ConversationId { get; private set; }
        public Conversation Conversation { get; private set; }
        public string Content { get; private set; } = null!;
        public int SenderId { get; private set; }
        public bool IsRead { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime SentAtUtc { get; private set; }
        public DateTime? ReadAtUtc { get; private set; }

        public void MarkRead()
        {
            IsRead = true;
            ReadAtUtc = DateTime.UtcNow;
        }
    }
}
