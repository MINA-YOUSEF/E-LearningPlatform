namespace E_LearningPlatform.Infrastructure.Security
{
    public static class CustomClaims
    {
        public const string MustChangePassword = "mustChangePassword";

        public const string IsActive = "isActive";

        public const string EmailConfirmed = "emailConfirmed";
        public const string SecurityStamp = "securityStamp";
    }
}
