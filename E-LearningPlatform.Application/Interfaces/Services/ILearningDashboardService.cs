using E_LearningPlatform.Application.DTOs.Dashboards.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ILearningDashboardService
    {
        Task<LearningDashboardResponseDto>
        GetDashboardAsync(
        CancellationToken cancellationToken = default);
    }
}
