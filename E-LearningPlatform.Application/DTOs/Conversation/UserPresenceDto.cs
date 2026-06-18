namespace E_LearningPlatform.Application.DTOs.Conversation
{
    public class UserPresenceDto
    {
        public int UserId { get; set; }

        public bool IsOnline { get; set; }

        public DateTime? LastSeenUtc { get; set; }
    }
}
