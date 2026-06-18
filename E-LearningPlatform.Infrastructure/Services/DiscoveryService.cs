namespace E_LearningPlatform.Infrastructure.Services
{
    using AutoMapper;
    using E_LearningPlatform.Application.Common;
    using E_LearningPlatform.Application.DTOs;
    using E_LearningPlatform.Application.DTOs.Discovery;
    using E_LearningPlatform.Application.Interfaces.Cache;
    using E_LearningPlatform.Application.Interfaces.Repositories;
    using E_LearningPlatform.Application.Interfaces.Services;
    using Microsoft.EntityFrameworkCore;

    public class DiscoveryService : IDiscoveryService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly ICacheService _cacheService;

        private readonly ICurrentUserService _currentUserService;

        public DiscoveryService (
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }

        public async Task<PagedResult<PopularCourseDto>> GetPopularCoursesAsync (PagedRequest request, CancellationToken cancellationToken = default)
        {
            var courses = await _unitOfWork.Courses

                .Query()
                    .AsNoTracking()
                    .Where(x =>
                    x.IsPublished &&
                    x.IsActive &&
                    x.EnrollmentCount > 0)
                    .OrderByDescending(x => x.EnrollmentCount)
                    .ThenByDescending(x => x.AverageRating)
                    .Skip((request.PageNumber - 1)
                    * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new PopularCourseDto
                    {
                        CourseId = x.Id,
                        Title = x.Title,
                        Slug = x.Slug.Value,
                        ThumbnailUrl =
                        x.Thumbnail != null
                        ? x.Thumbnail.Url
                        : null,
                        AverageRating = x.AverageRating,
                        RatingCount = x.RatingCount,
                        EnrollmentCount = x.EnrollmentCount,
                        Price = x.Price.Amount,
                        Currency = x.Price.Currency

                    })
                 .ToListAsync(cancellationToken);

            return new PagedResult<PopularCourseDto>
            {
                Items = courses,
                TotalCount = courses.Count,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<TopRatedCourseDto>> GetTopRatedCourseAsync (PagedRequest request, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"top-rated:{request.PageNumber}:{request.PageSize}";
            var cached = await _cacheService.GetAsync<PagedResult<TopRatedCourseDto>>(cacheKey);
            if (cached != null)
            { return cached; }
            var courses = await _unitOfWork.Courses.Query()
                .AsNoTracking()
                .Where(c => c.IsPublished && c.IsActive && c.RatingCount > 0)
                .OrderByDescending(c => c.AverageRating)
                .ThenByDescending(c => c.RatingCount)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new TopRatedCourseDto
                {
                    CourseId = x.Id,
                    Title = x.Title,
                    Slug = x.Slug.Value,
                    ThumbnailUrl = x.Thumbnail != null ? x.Thumbnail.Url : null,
                    AverageRating = x.AverageRating,
                    RatingCount = x.RatingCount,
                    Price = x.Price.Amount,
                    Currency = x.Price.Currency
                })
               .ToListAsync(cancellationToken);
            await _cacheService.SetAsync(cacheKey, new PagedResult<TopRatedCourseDto>(), TimeSpan.FromMinutes(10), cancellationToken);
            return new PagedResult<TopRatedCourseDto>
            {
                Items = courses,
                TotalCount = courses.Count
            };
        }


        public async Task<PagedResult<CourseCardDto>>
            SearchCoursesAsync (
            CourseSearchRequestDto request,
            CancellationToken cancellationToken = default)
        {
            if (request.PageNumber <= 0)
            {
                throw new ArgumentException(
                    "Invalid page number.");
            }

            if (request.PageSize <= 0)
            {
                throw new ArgumentException(
                    "Invalid page size.");
            }

            var query = _unitOfWork.Courses
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.IsPublished &&
                    x.IsActive);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.Title.Contains(search));
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(x =>
                    x.CategoryCourses.Any(c =>
                        c.CategoryId ==
                        request.CategoryId.Value));
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(x =>
                    x.Price.Amount >=
                    request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(x =>
                    x.Price.Amount <=
                    request.MaxPrice.Value);
            }

            if (request.MinRating.HasValue)
            {
                query = query.Where(x =>
                    x.AverageRating >=
                    request.MinRating.Value);
            }

            query = request.SortBy?.ToLower() switch
            {
                "price" =>
                    request.Descending
                        ? query.OrderByDescending(
                            x => x.Price.Amount)
                        : query.OrderBy(
                            x => x.Price.Amount),

                "rating" =>
                    query.OrderByDescending(
                        x => x.AverageRating),

                "popular" =>
                    query.OrderByDescending(
                        x => x.EnrollmentCount),

                "newest" =>
                    query.OrderByDescending(
                        x => x.CreatedAtUtc),

                _ =>
                    query.OrderByDescending(
                        x => x.CreatedAtUtc)
            };

            var totalCount = await query
                .CountAsync(cancellationToken);

            var courses = await query
                .Skip((request.PageNumber - 1)
                    * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CourseCardDto
                {
                    CourseId = x.Id,

                    Title = x.Title,

                    Slug = x.Slug.Value,

                    ThumbnailUrl =
                        x.Thumbnail != null
                            ? x.Thumbnail.Url
                            : null,

                    Price = x.Price.Amount,

                    Currency = x.Price.Currency,

                    AverageRating = x.AverageRating,

                    RatingCount = x.RatingCount,

                    EnrollmentCount = x.EnrollmentCount
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<CourseCardDto>
            {
                Items = courses,

                TotalCount = totalCount,

                PageNumber = request.PageNumber,

                PageSize = request.PageSize
            };
        }


    }
}
