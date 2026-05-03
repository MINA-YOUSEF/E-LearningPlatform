using E_LearningPlatform.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);

        Task<AuthResponseDto> ConfirmationEmailAsync(ConfirmationEmailRequestDto request, CancellationToken cancellationToken = default);

        Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
        Task<RegisterResponseDto> ResendConfirmationEmailAsync(ResendConfirmationRequestDto request, CancellationToken cancellationToken = default);
    }
}
