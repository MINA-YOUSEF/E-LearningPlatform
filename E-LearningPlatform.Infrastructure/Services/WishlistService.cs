using E_LearningPlatform.Application.DTOs.Wishlist;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services;

public class WishlistService : IWishlistService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public WishlistService (
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task AddCourseAsync (
        int courseId,
        CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        var userId = _currentUserService.UserId.Value;

        var wishlist = await _unitOfWork
            .WishLists
            .Query()
            .Include(x => x.Courses)
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (wishlist == null)
        {
            wishlist = new WishList(
                userId,
                _currentUserService.FullName);

            await _unitOfWork.WishLists
                .AddAsync(
                    wishlist,
                    cancellationToken);
        }

        var course = await _unitOfWork.Courses
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == courseId
                     && x.IsPublished
                     && x.IsActive,
                cancellationToken);

        if (course == null)
        {
            throw new InvalidOperationException(
                "Course not found.");
        }

        var exists = wishlist.Courses
            .Any(x => x.CourseId == courseId);

        if (exists)
        {
            throw new InvalidOperationException(
                "Course already exists in wishlist.");
        }

        wishlist.AddCourse(courseId);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    public async Task RemoveCourseAsync (
        int courseId,
        CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        var userId = _currentUserService.UserId.Value;

        var wishlist = await _unitOfWork
            .WishLists
            .Query()
            .Include(x => x.Courses)
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (wishlist == null)
        {
            throw new InvalidOperationException(
                "Wishlist not found.");
        }

        var item = wishlist.Courses
            .FirstOrDefault(
                x => x.CourseId == courseId);

        if (item == null)
        {
            throw new InvalidOperationException(
                "Course not found in wishlist.");
        }

        wishlist.RemoveCourse(courseId);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<List<WishlistCourseDto>>
        GetMyWishlistAsync (
            CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        return await _unitOfWork
            .WishListCourses
            .Query()
            .AsNoTracking()
            .Where(x =>
                x.WishList.UserId ==
                _currentUserService.UserId.Value)
            .Select(x =>
                new WishlistCourseDto
                {
                    CourseId = x.Course.Id,
                    Title = x.Course.Title,
                    Price = x.Course.Price.Amount,
                    ThumbnailUrl =
                        x.Course.Thumbnail.Url
                })
            .ToListAsync(
                cancellationToken);
    }
}