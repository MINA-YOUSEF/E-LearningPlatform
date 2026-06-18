using E_LearningPlatform.Application.DTOs.Dashboards.Instructor;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IInstructorDashboardService
    {
        Task<InstructorDashboardDto>
       GetDashboardAsync (
           CancellationToken cancellationToken = default);
    }
}
