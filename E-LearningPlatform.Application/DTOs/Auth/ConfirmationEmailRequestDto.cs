using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Auth
{
    public class ConfirmationEmailRequestDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
