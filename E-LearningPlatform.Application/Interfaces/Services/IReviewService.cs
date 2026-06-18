using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Review;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> AddReviewAsync(
            CreateReviewRequestDto request,
            CancellationToken cancellationToken = default);

        Task<ReviewResponseDto> UpdateReviewAsync(
            int reviewId,
            CreateReviewRequestDto request,
            CancellationToken cancellationToken = default);

        Task DeleteReviewAsync(
            int reviewId,
            CancellationToken cancellationToken = default);

        Task<PagedResult<ReviewResponseDto>> GetCourseReviewsAsync(
            int courseId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
