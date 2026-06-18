namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.DTOs.Wishlist;
    using E_LearningPlatform.Application.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]

    public partial class WishlistController : BaseV1Controller
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController (IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("{courseId}")]
        public async Task<IActionResult> dd (int courseId)
        {
            await _wishlistService
                .AddCourseAsync(courseId);

            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> Remove (int courseId)
        {
            await _wishlistService
                .RemoveCourseAsync(courseId);

            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<List<WishlistCourseDto>>> Get ()
        {
            return Ok(
                await _wishlistService
                    .GetMyWishlistAsync());
        }
    }
}
