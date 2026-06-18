namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.Common;
    using E_LearningPlatform.Application.DTOs.Order;
    using E_LearningPlatform.Application.Interfaces.Services;
    using E_LearningPlatform.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class OrderController : BaseV1Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentUserService;

        public OrderController (
            IOrderService orderService,
            ICurrentUserService currentUserService)
        {
            _orderService = orderService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Create new order
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>>
            Create (
            [FromBody] CreateOrderRequestDto request,
            CancellationToken cancellationToken)
        {
            var userId =
                _currentUserService.UserId!.Value;

            var result =
                await _orderService.CreateOrderAsync(
                    request,
                    userId,
                    cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { orderId = result.Id },
                result);
        }

        /// <summary>
        /// Get order details
        /// </summary>
        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderResponseDto>>
            GetById (
            int orderId,
            CancellationToken cancellationToken)
        {
            var result =
                await _orderService.GetOrderByIdAsync(
                    orderId,
                    cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get current user orders
        /// </summary>
        [HttpGet]
        public async Task<
            ActionResult<
                PagedResult<OrderResponseDto>>>
            GetMyOrders (
            CancellationToken cancellationToken)
        {
            var userId =
                _currentUserService.UserId!.Value;

            var result =
                await _orderService.GetUserOrdersAsync(
                    userId,
                    cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        [HttpPatch("{orderId:int}/cancel")]
        public async Task<IActionResult>
            Cancel (
            int orderId,
            CancellationToken cancellationToken)
        {
            await _orderService.CancelOrderAsync(
                orderId,
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Mark order as paid
        /// </summary>
        [Authorize(Policy = Policies.AdminFullAccess)]
        [HttpPatch("{orderId:int}/paid")]
        public async Task<IActionResult>
            MarkAsPaid (
            int orderId,
            CancellationToken cancellationToken)
        {
            await _orderService.MarkOrderAsPaidAsync(
                orderId,
                cancellationToken);

            return NoContent();
        }
    }
}
