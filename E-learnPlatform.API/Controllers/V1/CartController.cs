using E_LearningPlatform.Application.DTOs.Cart;
using E_LearningPlatform.Application.DTOs.Order;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace E_learnPlatform.API.Controllers.V1
{
    [Authorize(Policy = Policies.StudentFullAccess)]
    public class CartController : BaseV1Controller
    {
        private readonly ICartService _cartService;

        public CartController (ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add/{courseId:int}")]
        public async Task<IActionResult> AddToCart (
            int courseId,
            CancellationToken cancellationToken)
        {
            await _cartService.AddToCartAsync(
                courseId,
                cancellationToken);

            return Ok(new
            {
                message = "Course added to cart successfully."
            });
        }


        [HttpGet]
        public async Task<ActionResult<CartResponseDto>> GetCart (
            CancellationToken cancellationToken)
        {
            var result =
                await _cartService.GetCartAsync(cancellationToken);

            return Ok(result);
        }

        [HttpDelete("remove/{courseId:int}")]
        public async Task<IActionResult> RemoveFromCart (
            int courseId,
            CancellationToken cancellationToken)
        {
            await _cartService.RemoveFromCartAsync(
                courseId,
                cancellationToken);

            return NoContent();
        }


        [HttpPost("checkout")]
        public async Task<ActionResult<OrderResponseDto>> Checkout (
            CancellationToken cancellationToken)
        {
            var result =
                await _cartService.CheckoutAsync(cancellationToken);

            return Ok(result);
        }
    }
}
