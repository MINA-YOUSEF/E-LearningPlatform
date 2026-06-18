using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Jobs;
using E_LearningPlatform.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Jobs
{
    public class EmailJob : IEmailJob
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailJob> _logger;
        public EmailJob (IEmailService emailService, ILogger<EmailJob> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }
        public async Task SendCourseCompletedEmailAsync (string email, string courseTitle)
        {
            try
            {
                _logger.LogInformation(
                    "Sending course completion email to {Email} for course {CourseTitle}",
                    email,
                    courseTitle);

                await _emailService.SendEmailAsync(
                    email,
                    $"Congratulations on completing {courseTitle}!",
                    $"You have successfully completed the course: {courseTitle}. Keep up the great work!");

                _logger.LogInformation(
                    "Course completion email sent successfully to {Email} for course {CourseTitle}",
                    email,
                    courseTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed sending course completion email to {Email} for course {CourseTitle}",
                    email,
                    courseTitle);

                throw;
            }
        }
    }
}
