namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.DTOs.Dashboards.Instructor;
    using E_LearningPlatform.Application.Interfaces.Services;
    using E_LearningPlatform.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Policy = Policies.InstructorFullAccess)]
        public class InstructorDashboardController : BaseV1Controller
        {
            private readonly IInstructorDashboardService _dashboardService;

            public InstructorDashboardController (
                IInstructorDashboardService dashboardService)
            {
                _dashboardService = dashboardService;
            }

            /// <summary>
            /// Get instructor dashboard statistics
            /// </summary>
            [HttpGet]
            public async Task<ActionResult<InstructorDashboardDto>>
                GetDashboard (
                CancellationToken cancellationToken)
            {
                var result =
                    await _dashboardService
                    .GetDashboardAsync(
                        cancellationToken);

                return Ok(result);
            }
        }
}
