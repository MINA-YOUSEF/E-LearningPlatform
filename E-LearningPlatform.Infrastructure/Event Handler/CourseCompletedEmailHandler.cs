using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Jobs;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Infrastructure.Identity;
using E_LearningPlatform.Infrastructure.Wrapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Event_Handler
{
    public class CourseCompletedEmailHandler : INotificationHandler<DomainEventNotification<CourseCompletedEvent>>
    {
        private readonly UserManager<AppUser> _userManager;
        public CourseCompletedEmailHandler (UserManager<AppUser> userManager)
        {

            _userManager = userManager;
        }
        public async Task Handle (DomainEventNotification<CourseCompletedEvent> notification, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(notification.Notification.UserId.ToString());
            if (user == null)
            {
                return;
            }
            BackgroundJob.Enqueue<IEmailJob>(
                x => x.SendCourseCompletedEmailAsync(
                   user.Email!,
                    notification.Notification.CourseTitle));
            //  await _emailService.SendEmailAsync(user.Email!, "Course Completed", "Congratulations! You have completed the course " + notification.Notification.CourseTitle + ".", cancellationToken);
        }
    }
}
