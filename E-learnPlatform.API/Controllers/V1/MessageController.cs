namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.DTOs.Message;
    using E_LearningPlatform.Application.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MessageController : BaseV1Controller
    {
        private readonly IMessageService _messageService;

        public MessageController (
            IMessageService messageService)
        {
            _messageService = messageService;
        }


        [HttpPost]
        public async Task<ActionResult<MessageResponseDto>>
            Send (
            [FromBody] SendMessageRequestDto request,
            CancellationToken cancellationToken)
        {
            var result =
                await _messageService.SendAsync(
                    request,
                    cancellationToken);

            return Ok(result);
        }


        [HttpGet("conversation/{conversationId:int}")]
        public async Task<ActionResult<List<MessageResponseDto>>>
            GetConversationMessages (
            int conversationId,
            CancellationToken cancellationToken)
        {
            var result =
                await _messageService.GetMessagesAsync(
                    conversationId,
                    cancellationToken);

            return Ok(result);
        }


        [HttpPatch("{messageId:int}/read")]
        public async Task<IActionResult>
            MarkAsRead (
            int messageId,
            CancellationToken cancellationToken)
        {
            await _messageService.MarkAsReadAsync(
                messageId,
                cancellationToken);

            return NoContent();
        }


        [HttpGet("unread-count")]
        public async Task<ActionResult<object>>
            GetUnreadCount (
            CancellationToken cancellationToken)
        {
            var count =
                await _messageService.GetUnreadCountAsync(
                    cancellationToken);

            return Ok(new
            {
                unreadCount = count
            });
        }
    }
}
