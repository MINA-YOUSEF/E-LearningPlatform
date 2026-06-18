namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.DTOs.Dashboards.Student;
    using E_LearningPlatform.Application.Interfaces.Services;
    using E_LearningPlatform.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Policy = Policies.StudentFullAccess)]
    public class LearningDashboardController : BaseV1Controller
    {
        private readonly ILearningDashboardService _learningDashboardService;

        public LearningDashboardController (
            ILearningDashboardService learningDashboardService)
        {
            _learningDashboardService = learningDashboardService;
        }


        [HttpGet]
        public async Task<ActionResult<LearningDashboardResponseDto>>
            GetDashboard (
            CancellationToken cancellationToken)
        {
            var result =
                await _learningDashboardService
                .GetDashboardAsync(cancellationToken);

            return Ok(result);
        }
    }
}
