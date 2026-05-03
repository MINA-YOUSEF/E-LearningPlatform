using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.External
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    }
}
