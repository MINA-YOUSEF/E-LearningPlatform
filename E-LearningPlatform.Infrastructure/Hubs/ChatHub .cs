using E_LearningPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace E_LearningPlatform.Infrastructure.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUserPresenceService _presenceService;

        public ChatHub (
           IUserPresenceService presenceService)
        {
            _presenceService = presenceService;
        }

        public override async Task OnConnectedAsync ()
        {
            var userId = GetUserId();

            await _presenceService
                .UserConnectedAsync(
                    userId,
                    Context.ConnectionId);

            await Clients.All.SendAsync(
                "UserConnected",
                userId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync (
            Exception? exception)
        {
            var userId = GetUserId();

            await _presenceService
                .UserDisconnectedAsync(
                    userId,
                    Context.ConnectionId);

            await Clients.All.SendAsync(
                "UserDisconnected",
                userId);

            await base.OnDisconnectedAsync(
                exception);
        }

        public async Task SendTyping (
            int conversationId,
            int receiverId)
        {
            var senderId = GetUserId();

            await Clients
                .User(receiverId.ToString())
                .SendAsync(
                    "UserTyping",
                    new
                    {
                        ConversationId = conversationId,
                        SenderId = senderId
                    });
        }

        public async Task StopTyping (
            int conversationId,
            int receiverId)
        {
            var senderId = GetUserId();

            await Clients
                .User(receiverId.ToString())
                .SendAsync(
                    "UserStoppedTyping",
                    new
                    {
                        ConversationId = conversationId,
                        SenderId = senderId
                    });
        }
        public async Task<List<int>>
            GetOnlineUsers ()
        {
            return await _presenceService
                .GetOnlineUsersAsync();
        }
        private int GetUserId ()
        {
            var userId =
                Context.User?
                .FindFirst(
                    ClaimTypes.NameIdentifier)?
                .Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new HubException(
                    "Unauthorized.");
            }

            return int.Parse(userId);
        }
    }

}
