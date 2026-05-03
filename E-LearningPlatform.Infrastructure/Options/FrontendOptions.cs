using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Options
{
    public class FrontendOptions
    {
        public const string SectionName = "Frontend";

        public string ResetPasswordTemplate { get; set; } = string.Empty;
        public string EmailConfirmationTemplate { get; set; } = string.Empty;
    }
}
