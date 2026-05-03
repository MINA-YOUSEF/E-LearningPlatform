using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Bio
        { get; set; } = null!;
    }
}
