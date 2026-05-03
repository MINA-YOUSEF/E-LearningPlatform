using System;

namespace E_LearningPlatform.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        private RefreshToken() { }

        public RefreshToken(int userId, string tokenHash, DateTime expiresAtUtc)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAtUtc = expiresAtUtc;
            IsRevoked = false;
        }

        public int UserId { get; private set; }
        public string TokenHash { get; private set; } = null!;
        public DateTime ExpiresAtUtc { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAtUtc { get; private set; }
        public string? ReplacedByTokenHash { get; private set; }

        public void Revoke(string? replacedByTokenHash = null)
        {
            IsRevoked = true;
            RevokedAtUtc = DateTime.UtcNow;
            ReplacedByTokenHash = replacedByTokenHash;
            Touch();
        }
    }
}
