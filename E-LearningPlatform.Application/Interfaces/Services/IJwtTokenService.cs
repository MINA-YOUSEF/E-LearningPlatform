using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId, string email, string role, bool MustChangePassword,
            bool IsActive,
            bool EmailConfirmed, string securityStamp);
        string GenerateRefreshToken();
        DateTime GetAccessTokenExpiryUtc();

    }
}
