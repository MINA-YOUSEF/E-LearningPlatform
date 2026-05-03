using E_LearningPlatform.Application.DTOs.Password;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Identity;
using E_LearningPlatform.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IJwtTokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FrontendOptions _frontOptions;
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<AppUser> _userManager;

        public PasswordService(IJwtTokenService tokenService,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            IOptions<FrontendOptions> frontOptions,
            IOptions<JwtOptions> jwtOptions,
            UserManager<AppUser> userManager)
        {
            _tokenService = tokenService;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _frontOptions = frontOptions.Value;
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task ChangePasswordAsync(ChangePasswordRequestDto request, int userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            var stmpResult = await _userManager.UpdateSecurityStampAsync(user);
            if (!stmpResult.Succeeded)
            {
                throw new BadRequestException(string.Join(";", stmpResult.Errors.Select(e => e.Description)));
            }
            user.MustChangePassword = false;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new BadRequestException(string.Join("; ", updateResult.Errors.Select(e => e.Description)));
            }
            await RevokeAllRefreshTokensAsync(user.Id, cancellationToken);

        }

        public async Task ForgotPasswordAsync(ForgetPasswordRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || string.IsNullOrWhiteSpace(user.Email))
            {
                throw new BadRequestException("Invalid email .");
            }

            var email = user.Email;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var template = _frontOptions.ResetPasswordTemplate;
            var resetUrl = template.Replace("{token}", encodedToken)
             .Replace("{email}", Uri.EscapeDataString(request.Email));
            ;
            if (resetUrl.Contains('{') || resetUrl.Contains('}'))
            {
                throw new InvalidOperationException(
                    "ResetPasswordTemplate contains unresolved placeholders.");
            }


            await _emailService.SendEmailAsync(
                      user.Email,
                     "reset your password",
                      $"click here to reset: <a href='{resetUrl}'>Confirm Email</a>"
                     , cancellationToken);

        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new BadRequestException("Invalid Email.");
            string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(";", result.Errors.Select(x => x.Description)));
            }
            var stampResult = await _userManager.UpdateSecurityStampAsync(user);
            if (!stampResult.Succeeded)
            {
                throw new BadRequestException(string.Join("; ", stampResult.Errors.Select(e => e.Description)));
            }
            user.MustChangePassword = false;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new BadRequestException(string.Join("; ", updateResult.Errors.Select(e => e.Description)));
            }

            await RevokeAllRefreshTokensAsync(user.Id, cancellationToken);
        }

        public async Task ForceResetAsync(int userId, CancellationToken cancellationToken = default)
        {

            var user = await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new NotFoundException("User not found.");

            user.MustChangePassword = true;

            var stampResult = await _userManager.UpdateSecurityStampAsync(user);
            if (!stampResult.Succeeded)
            {
                throw new BadRequestException(string.Join("; ", stampResult.Errors.Select(e => e.Description)));
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            await RevokeAllRefreshTokensAsync(user.Id, cancellationToken);
        }



        private async Task RevokeAllRefreshTokensAsync(int userId, CancellationToken cancellationToken)
        {
            var tokens = await _unitOfWork.RefreshTokens.Query()
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
            {
                token.Revoke();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

    }
}
