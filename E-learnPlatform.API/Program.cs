using E_LearningPlatform.Application.Extensions;
using E_LearningPlatform.Application.Interfaces.Jobs.CleanUp;
using E_LearningPlatform.Application.Interfaces.Outbox;
using E_LearningPlatform.Infrastructure.Extensions;
using E_LearningPlatform.Infrastructure.Hubs;
using E_LearningPlatform.Infrastructure.Options;
using E_LearningPlatform.Infrastructure.Seed;
using E_learnPlatform.API.Extensions;
using E_learnPlatform.API.Middleware;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace E_learnPlatform.API
{
    public class Program
    {
        public static async Task Main (string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //builder.Services.
            // Add services to the container.
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
            //Register Jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ClockSkew = TimeSpan.FromMinutes(1)

                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken =
                            context.Request.Query["access_token"];

                        var path =
                            context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken)
                            &&
                            path.StartsWithSegments(
                                "/hubs/notifications"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };

            });
            builder.Services.AddHangfire(config =>

            {
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));

            });
            builder.Services.AddHangfireServer();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);


            });
            builder.Services.AddMyAuthorization();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var recurringJobManager =
                    scope.ServiceProvider
                        .GetRequiredService<
                            IRecurringJobManager>();

                recurringJobManager.AddOrUpdate<
                    INotificationsCleanUpJob>(
                        "NotificationsCleanup",
                        x => x.ExecuteAsync(),
                        Cron.Daily);

                recurringJobManager.AddOrUpdate<
                    IOutboxProcessor>(
                        "outbox",
                        x => x.ProcessAsync(),
                        Cron.Minutely);
            }

            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.MapOpenApi();
                app.UseHangfireDashboard("/hangfire");
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<NotificationHub>("/hubs/notifications");


            app.MapHub<ChatHub>(
                "/hubs/chat");
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();
            await AppDbInitializer.SeedAsync(app.Services);

            app.Run();
        }
    }
}
