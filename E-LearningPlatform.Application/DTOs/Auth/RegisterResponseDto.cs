using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public string Email { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}
