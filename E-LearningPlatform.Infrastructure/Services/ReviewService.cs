using AutoMapper;
using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Review;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;



namespace E_LearningPlatform.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public ReviewService (IUnitOfWork unitOfWork,
           IMapper mapper,
           ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<ReviewResponseDto> AddReviewAsync (CreateReviewRequestDto request, CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found.");
            }
            if (!course.IsPublished || !course.IsActive)
            {
                throw new InvalidOperationException(
                    "Course unavailable.");
            }
            var enrolled = await _unitOfWork
                .CourseEnrollment
                .Query()
                .AnyAsync(e =>
                e.CourseId == request.CourseId
                && e.UserId == _currentUserService.UserId,
                cancellationToken);
            if (!enrolled)
            {
                throw new InvalidOperationException("User must be enrolled in the course to leave a review.");
            }
            var alreadyReviewed = await _unitOfWork.Reviews
                     .Query()
                .AnyAsync(
                   x => x.UserId ==
                   _currentUserService.UserId &&
                   x.CourseId == request.CourseId,
                   cancellationToken
                   );

            if (alreadyReviewed)
            {
                throw new InvalidOperationException(
                    "You already reviewed this course.");
            }

            var review = new Review
            (
                request.CourseId,
                _currentUserService.UserId.Value,
                new Rating(request.Rating),
                    request.Comment,
                     request.Title
            );
            await _unitOfWork.Reviews
              .AddAsync(review, cancellationToken);
            course.UpdateRating(new Rating(request.Rating));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            review.AddDomainEvent(
    new ReviewAddedEvent(
        review.Id,
        course.Id,
        course.InstructorId,
        review.UserId,
        course.Title,
        review.Rating));
            return
                _mapper.Map<ReviewResponseDto>(review);
        }

        public async Task DeleteReviewAsync (
      int reviewId,
      CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");
            }

            var review = await _unitOfWork.Reviews
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == reviewId,
                    cancellationToken);

            if (review == null)
            {
                throw new InvalidOperationException(
                    "Review not found.");
            }

            if (review.UserId != _currentUserService.UserId.Value)
            {
                throw new UnauthorizedAccessException(
                    "You can only delete your own review.");
            }
            var course = await _unitOfWork.Courses
     .Query()
     .Include(x => x.Reviews)
     .FirstOrDefaultAsync(
         c => c.Id == review.CourseId,
         cancellationToken);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found.");
            }
            _unitOfWork.Reviews.Remove(review);
            course.RemoveReview(review);
            course.RecalculateAverageRating();
            review.AddDomainEvent(
       new AuditEvent(
           _currentUserService.UserId,
           "Delete",
           nameof(Review),
           review.Id,
           review,
           null));

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }

        public async Task<PagedResult<ReviewResponseDto>>
     GetCourseReviewsAsync (
     int courseId,
     int pageNumber,
     int pageSize,
     CancellationToken cancellationToken = default)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException(
                    "Invalid page number.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException(
                    "Invalid page size.");
            }

            var query = _unitOfWork.Reviews
                .Query()
                .Where(x => x.CourseId == courseId);

            var totalCount = await query
                .CountAsync(cancellationToken);

            var reviews = await query
                .OrderByDescending(x => x.CreatedAtUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var reviewDtos =
                _mapper.Map<List<ReviewResponseDto>>(reviews);

            return new PagedResult<ReviewResponseDto>
            {
                Items = reviewDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ReviewResponseDto>
     UpdateReviewAsync (
     int reviewId,
     CreateReviewRequestDto request,
     CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");
            }

            var review = await _unitOfWork.Reviews
                .Query()
                .Include(x => x.Course)
                .ThenInclude(x => x.Reviews)
                .FirstOrDefaultAsync(
                    x => x.Id == reviewId,
                    cancellationToken);

            if (review == null)
            {
                throw new InvalidOperationException(
                    "Review not found.");
            }

            if (review.UserId != _currentUserService.UserId.Value)
            {
                throw new UnauthorizedAccessException(
                    "You can only update your own review.");
            }

            review.Update(
                new Rating(request.Rating),
                request.Comment,
                request.Title);

            review.Course.RecalculateAverageRating();

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return _mapper.Map<ReviewResponseDto>(review);
        }
    }
}




