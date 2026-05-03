using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Options
{
    public class EmailOptions
    {
        public const string SectionName = "Email";

        public string SmtpHost { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;

    }
}
