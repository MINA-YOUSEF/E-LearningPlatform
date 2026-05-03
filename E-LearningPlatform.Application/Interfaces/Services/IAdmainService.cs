using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Admain;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IAdmainService
    {
        Task<PagedResult<UserManagementDto>> GetUsersAsync(PagedUserRequest request, CancellationToken cancellationToken);
        Task SetUserStatusAsync(int userId, bool isActive, CancellationToken cancellationToken = default);


    }

}
