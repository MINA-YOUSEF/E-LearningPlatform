namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.Common;
    using E_LearningPlatform.Application.DTOs.Review;
    using E_LearningPlatform.Application.Interfaces.Services;
    using E_LearningPlatform.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public partial class WishlistController
    {
        [Authorize]
        public class ReviewController : BaseV1Controller
        {
            private readonly IReviewService _reviewService;

            public ReviewController (
                IReviewService reviewService)
            {
                _reviewService = reviewService;
            }

            [Authorize(Policy = Policies.StudentFullAccess)]
            [HttpPost]
            public async Task<ActionResult<ReviewResponseDto>>
                AddReview (
                [FromBody] CreateReviewRequestDto request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _reviewService.AddReviewAsync(
                        request,
                        cancellationToken);

                return Ok(result);
            }



            [Authorize(Policy = Policies.StudentFullAccess)]
            [HttpPut("{reviewId:int}")]
            public async Task<ActionResult<ReviewResponseDto>>
                UpdateReview (
                int reviewId,
                [FromBody] CreateReviewRequestDto request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _reviewService.UpdateReviewAsync(
                        reviewId,
                        request,
                        cancellationToken);

                return Ok(result);
            }



            [Authorize(Policy = Policies.StudentFullAccess)]
            [HttpDelete("{reviewId:int}")]
            public async Task<IActionResult>
                DeleteReview (
                int reviewId,
                CancellationToken cancellationToken)
            {
                await _reviewService.DeleteReviewAsync(
                    reviewId,
                    cancellationToken);

                return NoContent();
            }

            [AllowAnonymous]
            [HttpGet("course/{courseId:int}")]
            public async Task<ActionResult<PagedResult<ReviewResponseDto>>>
                GetCourseReviews (
                int courseId,
                [FromQuery] PagedRequest request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _reviewService.GetCourseReviewsAsync(
                        courseId,
                        request.PageNumber,
                        request.PageSize,
                        cancellationToken);

                return Ok(result);
            }
        }
    }
}
