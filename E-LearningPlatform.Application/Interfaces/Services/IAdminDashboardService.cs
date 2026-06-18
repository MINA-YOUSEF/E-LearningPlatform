using E_LearningPlatform.Application.DTOs.Dashboards.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardDto>
            GetDashboardAsync (
                CancellationToken cancellationToken = default);
    }
}
