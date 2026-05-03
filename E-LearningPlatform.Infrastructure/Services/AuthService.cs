using E_LearningPlatform.Application.DTOs.Auth;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using E_LearningPlatform.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FrontendOptions _frontOptions;
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(IJwtTokenService tokenService,
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



        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new BadRequestException("Email is already registered.");
            }
            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FullName = request.FullName ?? request.UserName,
                Bio = request.Bio,
                CreatedAt = DateTime.UtcNow,
                MustChangePassword = false,
                IsActive = true
            };
            if (request.Role != RoleNames.Instructor && request.Role != RoleNames.Student)
            {
                throw new BadRequestException("Invalid role specified.");
            }
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"User creation failed: {errors}");
            }


            var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
            var tokenEmailConfirmation = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenEmailConfirmation));
            var template = _frontOptions.EmailConfirmationTemplate;
            var confirmationUrl = template.Replace("{token}", encodedToken)
                .Replace("{email}", Uri.EscapeDataString(request.Email));
            ;
            if (confirmationUrl.Contains('{') || confirmationUrl.Contains('}'))
            {
                throw new InvalidOperationException(
                    "EmailConfirmationTemplate contains unresolved placeholders.");
            }


            await _emailService.SendEmailAsync(
                      user.Email,
                     "Confirm your email",
                      $"click here to confirm: <a href='{confirmationUrl}'>Confirm Email</a>"
                     , cancellationToken);

            return new RegisterResponseDto
            {

                Email = user.Email,
                Message = "Registration successful. Please check your email to confirm your account."
            };

        }
        public async Task<AuthResponseDto> ConfirmationEmailAsync(ConfirmationEmailRequestDto request, CancellationToken cancellationToken = default)
        {
            var decodedBytes = WebEncoders.Base64UrlDecode(request.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BadRequestException("Invalid email.");
            }
            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!confirmResult.Succeeded)
            {
                var errors = string.Join("; ", confirmResult.Errors.Select(e => e.Description));
                throw new BadRequestException($"Email confirmation failed: {errors}");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return await BuildAuthResponseAsync(user, roles[0], cancellationToken);

        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }
            if (!user.EmailConfirmed)
                throw new ForbiddenException("Email not confirmed.");

            if (!user.IsActive)
                throw new ForbiddenException("Account is inactive.");

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }
            var roles = await _userManager.GetRolesAsync(user);

            if (user.MustChangePassword)
            {
                return new AuthResponseDto
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    Role = roles[0],
                    RequiresPasswordChange = true
                };
            }
            return await BuildAuthResponseAsync(user, roles[0], cancellationToken);
        }
        private async Task<AuthResponseDto> BuildAuthResponseAsync(
              AppUser user,
              string role,
              CancellationToken cancellationToken)
        {

            var accessToken = _tokenService.GenerateToken(
                user.Id,
                user.Email ?? string.Empty,
                role,
                user.MustChangePassword,
                user.IsActive,
                user.EmailConfirmed,
                user.SecurityStamp ?? string.Empty);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _unitOfWork.RefreshTokens.AddAsync(
                new RefreshToken(user.Id, HashToken(refreshToken), DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays)),
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                Role = role,
                RequiresPasswordChange = false,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAtUtc = _tokenService.GetAccessTokenExpiryUtc()
            };
        }


        private static string HashToken(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
        {
            var tokenHash = HashToken(request.RefreshToken);
            var stordToken = await _unitOfWork.RefreshTokens.Query()
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash && !x.IsRevoked);
            if (stordToken == null || stordToken.ExpiresAtUtc < DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid refresh token .");
            }
            var user = await _userManager.FindByIdAsync(stordToken.UserId.ToString());
            if (user is null || !user.IsActive)
            {
                throw new UnauthorizedException("Invalid refresh token owner.");
            }

            stordToken.Revoke();
            _unitOfWork.RefreshTokens.Update(stordToken);
            var roles = await _userManager.GetRolesAsync(user);
            return await BuildAuthResponseAsync(user, roles.FirstOrDefault() ?? string.Empty, cancellationToken);

        }

        public async Task<RegisterResponseDto> ResendConfirmationEmailAsync(ResendConfirmationRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BadRequestException("Invalid email or register again.");
            }
            if (!user.IsActive)
            {
                throw new ForbiddenException("Account is inactive.");
            }
            if (user.EmailConfirmed)
            {
                throw new BadRequestException("Email is already confirmed.");
            }
            var tokenEmailConfirmation = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenEmailConfirmation));
            var template = _frontOptions.EmailConfirmationTemplate;
            var confirmationUrl = template.Replace("{token}", encodedToken)
                .Replace("{email}", Uri.EscapeDataString(request.Email));
            if (confirmationUrl.Contains('{') || confirmationUrl.Contains('}'))
            {
                throw new InvalidOperationException(
                    "EmailConfirmationTemplate contains unresolved placeholders.");
            }

            await _emailService.SendEmailAsync(
                      user.Email,
                     "Confirm your email",
                      $"click here to confirm: <a href='{confirmationUrl}'>Confirm Email</a>"
                     , cancellationToken);

            return new RegisterResponseDto
            {

                Email = user.Email,
                Message = "Registration successful. Please check your email to confirm your account."
            };

        }
    }
}
