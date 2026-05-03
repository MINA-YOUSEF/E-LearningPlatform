using E_LearningPlatform.Application.DTOs.Password;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IPasswordService
    {
        Task ChangePasswordAsync(ChangePasswordRequestDto request, int userId, CancellationToken cancellationToken = default);
        Task ForgotPasswordAsync(ForgetPasswordRequestDto request, CancellationToken cancellationToken = default);
        Task ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default);
        Task ForceResetAsync(int userId, CancellationToken cancellationToken = default);
    }
}
