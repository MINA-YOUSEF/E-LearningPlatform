using E_LearningPlatform.Application.DTOs.Dashboards.Admin;
using E_LearningPlatform.Application.Interfaces.Cache;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICacheService _cacheService;
        public AdminDashboardService (IUnitOfWork unitOfWork, UserManager<AppUser> userManager, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _cacheService = cacheService;
        }

        public async Task<AdminDashboardDto> GetDashboardAsync (CancellationToken cancellationToken = default)
        {
            var cacheKey = "admin_dashboard_stats";
            var cachedStats = await _cacheService.GetAsync<AdminDashboardDto>(cacheKey);
            if (cachedStats != null)
            {
                return cachedStats;
            }
            var totalUsers = await _userManager.Users.CountAsync();
            var totalStudents = await _userManager.GetUsersInRoleAsync("Student");
            var totalCourses = await _unitOfWork.Courses.Query().CountAsync(cancellationToken);
            var publishedCourses = await _unitOfWork.Courses.Query().Where(c => c.IsPublished).CountAsync(cancellationToken);
            var pendingCourses =
                await _unitOfWork.Courses
                    .Query()
                    .CountAsync(x => x.ApprovalStatus == ApprovalStatus.Pending, cancellationToken);
            var totalRevenue = await _unitOfWork.Payments.Query().Where(x =>
            x.Status ==
            PaymentStatus.Paid)
        .SumAsync(
            x => x.Amount.Amount,
            cancellationToken);
            var totalEnrollments = await _unitOfWork.Enrollments.Query()
        .CountAsync(cancellationToken);
            var recentUsers = await _userManager.Users.OrderByDescending(x => x.CreatedAt).Take(10)
        .Select(x =>
            new RecentUserDto
            {
                UserId = x.Id,
                FullName = x.FullName,
                Email = x.Email!
            }).ToListAsync();
            var recentPayments = await _unitOfWork.Payments
        .Query()
        .OrderByDescending(
            x => x.CreatedAtUtc)
        .Take(10)
        .Select(x =>
            new RecentPaymentDto
            {
                PaymentId = x.Id,

                Amount =
                    x.Amount.Amount,

                Currency =
                    x.Amount.Currency,

                PaidAtUtc =
                    x.CreatedAtUtc
            })
        .ToListAsync(cancellationToken);
            var dashboardStats = new AdminDashboardDto
            {
                TotalUsers = totalUsers,
                TotalStudents = totalStudents.Count,
                TotalInstructors = totalUsers - totalStudents.Count,
                TotalCourses = totalCourses,
                PublishedCourses = publishedCourses,
                PendingCourses = pendingCourses,
                TotalRevenue = totalRevenue,
                TotalEnrollments = totalEnrollments,
                RecentUsers = recentUsers,
                RecentPayments = recentPayments
            };
            await _cacheService.SetAsync(cacheKey, dashboardStats, TimeSpan.FromMinutes(2));
            return dashboardStats;
        }
    }
}
