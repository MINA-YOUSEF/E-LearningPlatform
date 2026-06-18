namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.Common;
    using E_LearningPlatform.Application.DTOs.Notification;
    using E_LearningPlatform.Application.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public partial class WishlistController
    {
        [Authorize]
        public class NotificationController : BaseV1Controller
        {
            private readonly INotificationService _notificationService;

            public NotificationController (
                INotificationService notificationService)
            {
                _notificationService = notificationService;
            }


            [HttpGet]
            public async Task<
                ActionResult<
                    PagedResult<NotificationResponseDto>>>
                GetNotifications (
                [FromQuery] PagedRequest request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _notificationService
                    .GetUserNotificationsAsync(
                        request,
                        cancellationToken);

                return Ok(result);
            }

            [HttpGet("unread-count")]
            public async Task<ActionResult<object>>
                GetUnreadCount (
                CancellationToken cancellationToken)
            {
                var count =
                    await _notificationService
                    .GetUnreadCountAsync(
                        cancellationToken);

                return Ok(new
                {
                    unreadCount = count
                });
            }


            [HttpPatch("{notificationId:int}/read")]
            public async Task<IActionResult>
                MarkAsRead (
                int notificationId,
                CancellationToken cancellationToken)
            {
                await _notificationService
                    .MarkAsReadAsync(
                        notificationId,
                        cancellationToken);

                return NoContent();
            }

            [HttpPatch("read-all")]
            public async Task<IActionResult>
                MarkAllAsRead (
                CancellationToken cancellationToken)
            {
                await _notificationService
                    .MarkAllAsReadAsync(
                        cancellationToken);

                return NoContent();
            }
        }
    }
}
