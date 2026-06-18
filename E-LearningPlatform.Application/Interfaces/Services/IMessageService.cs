using E_LearningPlatform.Application.DTOs.Message;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IMessageService
    {
        Task<MessageResponseDto> SendAsync (
            SendMessageRequestDto request,
            CancellationToken cancellationToken = default);

        Task<List<MessageResponseDto>>
            GetMessagesAsync (
            int conversationId,
            CancellationToken cancellationToken = default);

        Task MarkAsReadAsync (
            int messageId,
            CancellationToken cancellationToken = default);

        Task<int> GetUnreadCountAsync (
            CancellationToken cancellationToken = default);
    }
}