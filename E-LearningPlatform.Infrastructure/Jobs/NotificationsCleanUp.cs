using E_LearningPlatform.Application.Interfaces.Jobs.CleanUp;
using E_LearningPlatform.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Jobs
{
    public class NotificationsCleanUpJob : INotificationsCleanUpJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationsCleanUpJob> _logger;
        public NotificationsCleanUpJob (IUnitOfWork unitOfWork, ILogger<NotificationsCleanUpJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task ExecuteAsync ()
        {
            try
            {
                var cutOffDate =
                    DateTime.UtcNow.AddMonths(-6);

                _logger.LogInformation(
                    "Notifications cleanup started. CutOffDate: {CutOffDate}",
                    cutOffDate);

                var deletedCount =
                    await _unitOfWork.Notifications
                        .Query()
                        .Where(x =>
                            x.CreatedAtUtc < cutOffDate)
                        .ExecuteDeleteAsync(
                            );

                _logger.LogInformation(
                    "Notifications cleanup completed. Deleted {DeletedCount} notifications",
                    deletedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Notifications cleanup failed");

                throw;
            }

        }
    }
}
