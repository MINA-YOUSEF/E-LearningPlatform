using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string Email { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}
