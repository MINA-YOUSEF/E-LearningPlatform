using E_LearningPlatform.Application.DTOs.Conversation;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace E_learnPlatform.API.Controllers.V1
{
    [Authorize]
    public class ConversationController : BaseV1Controller
    {
        private readonly IConversationService _conversationService;

        public ConversationController (
            IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        /// <summary>
        /// Create conversation between student and instructor
        /// </summary>
        [Authorize(Policy = Policies.StudentFullAccess)]
        [HttpPost]
        public async Task<ActionResult<ConversationDto>>
            Create (
            [FromQuery] int instructorId,
            [FromQuery] int courseId,
            CancellationToken cancellationToken)
        {
            var result =
                await _conversationService.CreateAsync(
                    instructorId,
                    courseId,
                    cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { conversationId = result.Id },
                result);
        }

        /// <summary>
        /// Get all my conversations
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ConversationDto>>>
            GetMyConversations (
            CancellationToken cancellationToken)
        {
            var result =
                await _conversationService
                .GetMyConversationsAsync(
                    cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get conversation details
        /// </summary>
        [HttpGet("{conversationId:int}")]
        public async Task<ActionResult<ConversationDto>>
            GetById (
            int conversationId,
            CancellationToken cancellationToken)
        {
            var result =
                await _conversationService
                .GetByIdAsync(
                    conversationId,
                    cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Close conversation
        /// </summary>
        [HttpPatch("{conversationId:int}/close")]
        public async Task<IActionResult>
            Close (
            int conversationId,
            CancellationToken cancellationToken)
        {
            await _conversationService.CloseAsync(
                conversationId,
                cancellationToken);

            return NoContent();
        }
    }
}
