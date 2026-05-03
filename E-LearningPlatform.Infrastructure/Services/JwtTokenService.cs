using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Options;
using E_LearningPlatform.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _options;
        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public string GenerateToken(int userId,
            string email,
            string role,
            bool MustChangePassword,
            bool IsActive,
            bool EmailConfirmed,
            string securityStamp)
        {
            var claims = new List<Claim>
            {
                new( ClaimTypes.NameIdentifier, userId.ToString() ),
                new(ClaimTypes.Email, email ) ,
                new( ClaimTypes.Role, role ),
                new(CustomClaims.SecurityStamp, securityStamp),
                new (CustomClaims.IsActive, IsActive.ToString()),
                new (CustomClaims.EmailConfirmed, EmailConfirmed.ToString()),
                new (CustomClaims.MustChangePassword, MustChangePassword.ToString()),
                new(JwtRegisteredClaimNames.Sub,userId.ToString()),
                new(JwtRegisteredClaimNames.Email,email) ,
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var signingKey = _options.GetSigningKey();
            if (string.IsNullOrWhiteSpace(signingKey))
            {
                throw new InvalidOperationException("JWT signing key is not configured.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                issuer: _options.Issuer,
                audience: _options.Audience,
                expires: GetAccessTokenExpiryUtc());
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public DateTime GetAccessTokenExpiryUtc()
        {
            return DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);
        }
    }
}
