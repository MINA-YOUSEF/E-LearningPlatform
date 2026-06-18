using E_LearningPlatform.Application.Interfaces.Cache;
using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Jobs.CleanUp;
using E_LearningPlatform.Application.Interfaces.Outbox;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Data;
using E_LearningPlatform.Infrastructure.Identity;
using E_LearningPlatform.Infrastructure.Jobs;
using E_LearningPlatform.Infrastructure.Options;
using E_LearningPlatform.Infrastructure.Repositories;
using E_LearningPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_LearningPlatform.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
            services.Configure<FrontendOptions>(configuration.GetSection(FrontendOptions.SectionName));
            services.Configure<CloudinaryOptions>(configuration.GetSection(CloudinaryOptions.SectionName));
            services.Configure<CurrencyOptions>(
    configuration.GetSection(CurrencyOptions.SectionName));
            //Register DbContext 
            services.AddDbContext<AppDbContext>(option =>
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //Register Identity
            services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                       .AddEntityFrameworkStores<AppDbContext>()
                       .AddDefaultTokenProviders();

            services.AddHttpContextAccessor();

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<ICourseAuthorizationService, CourseAuthorizationService>();
            services.AddScoped<ICertificatePdfGenerator, CertificatePdfGenerator>();
            services.AddSignalR();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<INotificationsCleanUpJob, NotificationsCleanUpJob>();
            services.AddScoped<ICartService, CartService>();

            services.AddScoped<IConversationService, ConversationService>();

            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<IReviewService, ReviewService>();

            services.AddScoped<IRefundService, RefundService>();

            services.AddScoped<IProgressService, ProgressService>();

            services.AddScoped<IUserPresenceService, UserPresenceService>();

            services.AddScoped<ICourseApprovalService, CourseApprovalService>();

            services.AddScoped<IDiscoveryService, DiscoveryService>();

            services.AddScoped<IInstructorDashboardService, InstructorDashboardService>();

            services.AddScoped<ILearningDashboardService, LearningDashboardService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICacheService, CacheService>();

            services.AddScoped<
                IOutboxProcessor,
                OutboxProcessor>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(
                    typeof(ServiceCollectionExtensions)
                        .Assembly);
            });


            return services;
        }
    }
}
