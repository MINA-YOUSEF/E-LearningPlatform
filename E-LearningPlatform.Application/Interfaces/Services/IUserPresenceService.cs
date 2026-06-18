// IUserPresenceService.cs

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IUserPresenceService
    {
        Task UserConnectedAsync (
        int userId,
        string connectionId,
        CancellationToken cancellationToken = default);

        Task UserDisconnectedAsync (
           int userId,
           string connectionId,
           CancellationToken cancellationToken = default);

        Task<bool> IsOnlineAsync (
            int userId,
            CancellationToken cancellationToken = default);

        Task<List<int>> GetOnlineUsersAsync (
            CancellationToken cancellationToken = default);
    }

}
