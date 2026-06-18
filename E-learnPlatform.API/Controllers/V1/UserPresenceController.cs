namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
        public class UserPresenceController : BaseV1Controller
        {
            private readonly IUserPresenceService _userPresenceService;

            public UserPresenceController (
                IUserPresenceService userPresenceService)
            {
                _userPresenceService = userPresenceService;
            }

            /// <summary>
            /// Check if user is online
            /// </summary>
            [HttpGet("{userId:int}/online")]
            public async Task<ActionResult<object>>
                IsOnline (
                int userId,
                CancellationToken cancellationToken)
            {
                var result =
                    await _userPresenceService
                    .IsOnlineAsync(
                        userId,
                        cancellationToken);

                return Ok(new
                {
                    userId,
                    isOnline = result
                });
            }

            /// <summary>
            /// Get all online users
            /// </summary>
            [HttpGet("online-users")]
            public async Task<ActionResult<List<int>>>
                GetOnlineUsers (
                CancellationToken cancellationToken)
            {
                var result =
                    await _userPresenceService
                    .GetOnlineUsersAsync(
                        cancellationToken);

                return Ok(result);
            }
        }
    }
