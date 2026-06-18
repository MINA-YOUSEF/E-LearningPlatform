using E_LearningPlatform.Application.DTOs.Dashboards.Instructor;
using E_LearningPlatform.Application.Interfaces.Cache;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class InstructorDashboardService : IInstructorDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cache;
        public InstructorDashboardService (IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _cache = cacheService;
        }
        public async Task<InstructorDashboardDto> GetDashboardAsync (CancellationToken cancellationToken = default)
        {
            var instructorId = _currentUserService.UserId!.Value;
            if (instructorId == null)
            {
                throw new UnauthorizedAccessException("User must be logged in to access the dashboard.");
            }
            var cacheKey = $"instructor_dashboard_{instructorId}";
            var cachedDashboard =
                await _cache.GetAsync<InstructorDashboardDto>(
                    cacheKey,
                    cancellationToken);
            if (cachedDashboard != null) { return cachedDashboard; }
            var courses = await _unitOfWork.Courses.Query()
            .Where(c => c.InstructorId == instructorId)
            .ToListAsync(cancellationToken: cancellationToken);

            var totalCourses = courses.Count;
            var publishedCourses = courses.Count(x => x.IsPublished);
            var totalStudents =
                 courses.Sum(x => x.EnrollmentCount);
            var averageRating =
            courses.Count == 0
                 ? 0
                 : courses.Average(
                  x => x.AverageRating);
            var revenue =
                await _unitOfWork.Enrollments
                .Query()
                .Where(x => courses.Select(courses => courses.Id).Contains(x.CourseId)).
                SumAsync(x => x.Price.Amount, cancellationToken: cancellationToken);
            var topCourses = courses
                .OrderByDescending(
                 x => x.EnrollmentCount)
                .Take(5)
                .Select(x =>
                new TopCourseDto
                {
                    CourseId = x.Id,

                    Title = x.Title,

                    EnrollmentCount =
                   x.EnrollmentCount,

                    Revenue =
                x.EnrollmentCount *
                x.Price.Amount,

                    Rating =
                x.AverageRating
                }).ToList();
            var recentReviews =
    await _unitOfWork.Reviews
        .Query()
        .Include(x => x.Course)
        .Where(x =>
            x.Course.InstructorId ==
            instructorId)
        .OrderByDescending(
            x => x.CreatedAtUtc)
        .Take(10)
        .Select(x =>
            new RecentReviewDto
            {
                CourseTitle =
                    x.Course.Title,

                Rating =
                    x.Rating,

                Comment =
                    x.Comment,

                CreatedAtUtc =
                    x.CreatedAtUtc
            })
        .ToListAsync(cancellationToken);

            var instructorDashboard = new InstructorDashboardDto
            {
                TotalRevenue = revenue,

                TotalStudents = totalStudents,

                TotalCourses = totalCourses,

                PublishedCourses = publishedCourses,

                AverageRating = averageRating,

                TopCourses = topCourses
            };
            await _cache.SetAsync(
                cacheKey,
                instructorDashboard,
                TimeSpan.FromMinutes(5),
                cancellationToken);
            return instructorDashboard;
        }
    }
}
