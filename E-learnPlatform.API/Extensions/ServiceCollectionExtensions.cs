using E_LearningPlatform.Infrastructure.Identity;
using E_LearningPlatform.Infrastructure.Security;

namespace E_learnPlatform.API.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddMyAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
             {
                 options.AddPolicy(Policies.PasswordChangedRequired, policy =>
                 {
                     policy.RequireAuthenticatedUser();

                     policy.RequireClaim(
                         CustomClaims.MustChangePassword,
                         "False");
                 });
                 options.AddPolicy(Policies.AdminFullAccess, policy =>
                 {
                     policy.RequireAuthenticatedUser();

                     policy.RequireRole(RoleNames.Admin);

                     policy.RequireClaim(
                         CustomClaims.MustChangePassword,
                         "False");

                     policy.RequireClaim(
                         CustomClaims.IsActive,
                         "True");

                     policy.RequireClaim(
                         CustomClaims.EmailConfirmed,
                         "True");
                 });
                 options.AddPolicy(Policies.InstructorFullAccess, policy =>
                 {
                     policy.RequireAuthenticatedUser();

                     policy.RequireRole(RoleNames.Instructor);

                     policy.RequireClaim(
                         CustomClaims.MustChangePassword,
                         "False");

                     policy.RequireClaim(
                         CustomClaims.IsActive,
                         "True");

                     policy.RequireClaim(
                         CustomClaims.EmailConfirmed,
                         "True");
                 });
                 options.AddPolicy(Policies.StudentFullAccess, policy =>
                 {
                     policy.RequireAuthenticatedUser();

                     policy.RequireRole(RoleNames.Student);

                     policy.RequireClaim(
                         CustomClaims.MustChangePassword,
                         "False");

                     policy.RequireClaim(
                         CustomClaims.IsActive,
                         "True");

                     policy.RequireClaim(
                         CustomClaims.EmailConfirmed,
                         "True");
                 });
                 options.AddPolicy(Policies.AdminOrInstructor, policy =>
                 {
                     policy.RequireAuthenticatedUser();

                     policy.RequireRole(
                         RoleNames.Admin,
                         RoleNames.Instructor);

                     policy.RequireClaim(
                         CustomClaims.MustChangePassword,
                         "False");

                     policy.RequireClaim(
                         CustomClaims.IsActive,
                         "True");

                     policy.RequireClaim(
                         CustomClaims.EmailConfirmed,
                         "True");
                 });
                 options.AddPolicy(Policies.FullAccess, policy =>
                 {
                     policy.RequireAuthenticatedUser();

                     policy.RequireClaim(CustomClaims.MustChangePassword, "False");

                     policy.RequireClaim(CustomClaims.IsActive, "True");

                     policy.RequireClaim(CustomClaims.EmailConfirmed, "True");
                 });
             });
            return services;
        }
    }
}
