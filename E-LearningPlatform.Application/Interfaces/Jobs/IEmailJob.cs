using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Jobs
{
    public interface IEmailJob
    {
        Task SendCourseCompletedEmailAsync (string email, string courseTitle);
    }
}
