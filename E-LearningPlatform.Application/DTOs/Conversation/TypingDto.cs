namespace E_LearningPlatform.Application.DTOs.Conversation
{
    public class TypingDto
    {
        public int ConversationId { get; set; }

        public int UserId { get; set; }

        public bool IsTyping { get; set; }
    }
}
