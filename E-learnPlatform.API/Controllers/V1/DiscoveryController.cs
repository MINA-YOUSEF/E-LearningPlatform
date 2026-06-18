namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.Common;
    using E_LearningPlatform.Application.DTOs;
    using E_LearningPlatform.Application.DTOs.Discovery;
    using E_LearningPlatform.Application.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
        public class DiscoveryController : BaseV1Controller
        {
            private readonly IDiscoveryService _discoveryService;

            public DiscoveryController (
                IDiscoveryService discoveryService)
            {
                _discoveryService = discoveryService;
            }

            /// <summary>
            /// Search courses with filters
            /// </summary>
            [HttpGet("search")]
            public async Task<ActionResult<PagedResult<CourseCardDto>>>
                Search (
                [FromQuery] CourseSearchRequestDto request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _discoveryService.SearchCoursesAsync(
                        request,
                        cancellationToken);

                return Ok(result);
            }

            /// <summary>
            /// Get most popular courses
            /// </summary>
            [HttpGet("popular")]
            public async Task<ActionResult<PagedResult<PopularCourseDto>>>
                GetPopular (
                [FromQuery] PagedRequest request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _discoveryService.GetPopularCoursesAsync(
                        request,
                        cancellationToken);

                return Ok(result);
            }

            /// <summary>
            /// Get top rated courses
            /// </summary>
            [HttpGet("top-rated")]
            public async Task<ActionResult<PagedResult<TopRatedCourseDto>>>
                GetTopRated (
                [FromQuery] PagedRequest request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _discoveryService.GetTopRatedCourseAsync(
                        request,
                        cancellationToken);

                return Ok(result);
            }
        }
    }
