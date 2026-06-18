using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_learnPlatform.API.Controllers.V1
{
    
    public class DashboardController : BaseV1Controller
    {
        ILearningDashboardService _learningDashboardService;

        public DashboardController(ILearningDashboardService learningDashboardService)
        {
            _learningDashboardService = learningDashboardService;
        }

        [Authorize(Policy = Policies.StudentFullAccess)]
        [HttpGet("student")]
        public async Task<IActionResult> GetDashboard(
    CancellationToken cancellationToken)
        {
            var result = await _learningDashboardService
                .GetDashboardAsync(cancellationToken);

            return Ok(result);
        }
    }
}
