using E_LearningPlatform.Domain.Entities.E_LearningPlatform.Domain.Entities;

namespace E_LearningPlatform.Domain.Entities
{
    public class Message : BaseEntity
    {
        private Message () { }

        public Message (
            int conversationId,
            int senderId,
            int receiverId,
            string content)
        {
            ConversationId = conversationId;

            SenderId = senderId;

            ReceiverId = receiverId;

            Content = content;

            IsRead = false;

            IsEdited = false;
        }

        public int ConversationId { get; private set; }

        public Conversation Conversation { get; private set; } = null!;

        public int SenderId { get; private set; }

        public int ReceiverId { get; private set; }

        public string Content { get; private set; } = null!;

        public bool IsRead { get; private set; }

        public DateTime? ReadAt { get; private set; }

        public bool IsEdited { get; private set; }

        public DateTime? EditedAt { get; private set; }

        public void MarkAsRead ()
        {
            IsRead = true;

            ReadAt = DateTime.UtcNow;
        }

        public void Edit (string content)
        {
            Content = content;

            IsEdited = true;

            EditedAt = DateTime.UtcNow;
        }
    }
}

