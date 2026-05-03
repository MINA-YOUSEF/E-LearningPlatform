using E_LearningPlatform.Application.DTOs.Password;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_learnPlatform.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        public IPasswordService _passwordService;
        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            int.TryParse(userId, out var id);

            // var result =

            await _passwordService.ChangePasswordAsync(request, id, cancellationToken);

            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequestDto request, CancellationToken cancellationToken)
        {

            await _passwordService.ForgotPasswordAsync(request, cancellationToken);

            return Ok();

        }
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request, CancellationToken cancellationToken)
        {

            await _passwordService.ResetPasswordAsync(request, cancellationToken);

            return Ok();
        }

        [Authorize(Policies.AdminFullAccess)]
        [HttpPost("force-reset/{userId}")]
        public async Task<IActionResult> ForceReset(int userId, CancellationToken cancellationToken)
        {
            await _passwordService.ForceResetAsync(userId, cancellationToken);
            return Ok();
        }
    }
}
