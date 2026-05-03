using E_LearningPlatform.Application.DTOs.Auth;
using E_LearningPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_learnPlatform.API.Controllers.V1
{

    public class AuthController : BaseV1Controller
    {


        private readonly IAuthService _authservice;
        public AuthController(IAuthService authservice)
        {
            _authservice = authservice;
        }
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
        {
            var response = await _authservice.RegisterAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("confirm-email")]
        public async Task<ActionResult<AuthResponseDto>> ConfirmEmail([FromBody] ConfirmationEmailRequestDto request, CancellationToken cancellationToken)
        {
            var response = await _authservice.ConfirmationEmailAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<ActionResult<RegisterResponseDto>> ResendConfirmationEmail([FromBody] ResendConfirmationRequestDto request, CancellationToken cancellationToken)
        {
            var response = await _authservice.ResendConfirmationEmailAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("login")]
       // [Authorize(policy: "PasswordChangedRequired")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
        {
            var response = await _authservice.LoginAsync(request, cancellationToken);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            var response = await _authservice.RefreshTokenAsync(request, cancellationToken);
            return Ok(response);
        }
    }
}