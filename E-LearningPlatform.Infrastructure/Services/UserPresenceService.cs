 
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class UserPresenceService : IUserPresenceService
    {
        private readonly IUnitOfWork _unitOfWork;

     public UserPresenceService (
        IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UserConnectedAsync (
            int userId,
            string connectionId,
            CancellationToken cancellationToken = default)
        {
            var onlineUser =
                await _unitOfWork.OnlineUsers
                .Query()
                .FirstOrDefaultAsync(
                    x => x.UserId == userId,
                    cancellationToken);

            if (onlineUser == null)
            {
                onlineUser = new OnlineUser(
                    userId,
                    connectionId);

                await _unitOfWork.OnlineUsers
                    .AddAsync(
                        onlineUser,
                        cancellationToken);
            }
            else
            {
                onlineUser.Reconnect(connectionId);
            }

            await _unitOfWork
                .SaveChangesAsync(
                    cancellationToken);
        }

        public async Task UserDisconnectedAsync (
            int userId,
            string connectionId,
            CancellationToken cancellationToken = default)
        {
            var onlineUser =
                await _unitOfWork.OnlineUsers
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                    x.UserId == userId
                    &&
                    x.ConnectionId == connectionId,
                    cancellationToken);

            if (onlineUser == null)
            {
                return;
            }

            onlineUser.Disconnect();

            await _unitOfWork
                .SaveChangesAsync(
                    cancellationToken);
        }

        public async Task<bool> IsOnlineAsync (
            int userId,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .OnlineUsers
                .Query()
                .AnyAsync(
                    x =>
                    x.UserId == userId
                    &&
                    x.IsOnline,
                    cancellationToken);
        }

        public async Task<List<int>>
            GetOnlineUsersAsync (
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .OnlineUsers
                .Query()
                .Where(x => x.IsOnline)
                .Select(x => x.UserId)
                .ToListAsync(
                    cancellationToken);
        }
    }
 
}
