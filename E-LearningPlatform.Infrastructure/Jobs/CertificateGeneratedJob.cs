using E_LearningPlatform.Application.Interfaces.Jobs;
using E_LearningPlatform.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Jobs
{
    public class CertificateGeneratedJob : ICertificateGeneratedJob
    {
        private readonly ICertificateService _certificateService;
        private readonly ILogger<CertificateGeneratedJob> _logger;
        public CertificateGeneratedJob (ICertificateService certificateService, ILogger<CertificateGeneratedJob> logger)
        {
            _certificateService = certificateService;
            _logger = logger;
        }
        public async Task CertificateGeneratedAsync (
     int courseId,
     CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    $"Certificate generation started for course {courseId}",
                    courseId);

                await _certificateService
                    .GenerateCertificateAsync(
                        courseId,
                        cancellationToken);

                _logger.LogInformation(
                    $"Certificate generation completed for course {courseId}",
                    courseId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"Certificate generation failed for course {courseId}",
                    courseId);

                throw;
            }
        }
    }
}
