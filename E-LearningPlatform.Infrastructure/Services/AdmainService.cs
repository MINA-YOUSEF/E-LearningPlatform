using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Admain;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class AdmainService : IAdmainService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public AdmainService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {

            _unitOfWork = unitOfWork;
            _userManager = userManager;

        }

        public async Task<PagedResult<UserManagementDto>> GetUsersAsync(PagedUserRequest request, CancellationToken cancellationToken)
        {
            IQueryable<AppUser> query;

            if (!string.IsNullOrEmpty(request.Role))
            {
                var usersInRole =
                    await _userManager.GetUsersInRoleAsync(request.Role);

                query = usersInRole.AsQueryable();
            }
            else
            {
                query = _userManager.Users;
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(u =>
                    u.IsActive == request.IsActive.Value);
            }

            if (request.EmailConfirmed.HasValue)
            {
                query = query.Where(u =>
                    u.EmailConfirmed == request.EmailConfirmed.Value);
            }

            if (request.CreatedFrom.HasValue)
            {
                query = query.Where(u =>
                    u.CreatedAt >= request.CreatedFrom.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var keyword = request.Search.Trim();

                query = query.Where(u =>
                    u.UserName.Contains(keyword) ||
                    (u.FullName != null &&
                     u.FullName.Contains(keyword)) ||
                    u.Email.Contains(keyword));
            }

            var totalCount =
                await query.CountAsync(cancellationToken);

            var users =
                await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var items =
                new List<UserManagementDto>(users.Count);

            foreach (var user in users)
            {
                var roles =
                    await _userManager.GetRolesAsync(user);

                items.Add(new UserManagementDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    ImageUrl = user.ImageUrl,
                    Role = roles.FirstOrDefault()
                });
            }

            return new PagedResult<UserManagementDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task SetUserStatusAsync(int userId, bool isActive, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new BadRequestException("User not found.");
            }
            user.IsActive = isActive;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join("; ", result.Errors.Select(x => x.Description)));

            }
        }



    }
}
