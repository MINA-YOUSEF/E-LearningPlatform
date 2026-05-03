using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;

        // Primary key from configuration ("Jwt:Key")
        public string Key { get; set; } = null!;

        // Backward-compatibility with previous name
        public string SecretKey { get; set; } = null!;

        public int AccessTokenMinutes { get; set; } = 60;
        public int RefreshTokenDays { get; set; } = 3;

        public string GetSigningKey()
            => !string.IsNullOrWhiteSpace(Key) ? Key : SecretKey;
    }
}

