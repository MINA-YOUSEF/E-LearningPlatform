using E_LearningPlatform.Application.DTOs.Conversation;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IConversationService
    {
        Task<ConversationDto> CreateAsync (
            int instructorId,
            int courseId,
            CancellationToken cancellationToken = default);

        Task<List<ConversationDto>> GetMyConversationsAsync (
            CancellationToken cancellationToken = default);

        Task<ConversationDto> GetByIdAsync (
            int conversationId,
            CancellationToken cancellationToken = default);

        Task CloseAsync (
            int conversationId,
            CancellationToken cancellationToken = default);
    }
}