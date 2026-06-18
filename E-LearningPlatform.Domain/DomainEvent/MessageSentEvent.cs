namespace E_LearningPlatform.Domain.DomainEvent
{
    public sealed class MessageSentEvent
    : BaseDomainEvent
    {
        public MessageSentEvent (
            int senderId,
            int receiverId,
            string content)
        {
            SenderId = senderId;

            ReceiverId = receiverId;

            Content = content;
        }

        public int SenderId { get; }

        public int ReceiverId { get; }

        public string Content { get; }
    }
}
