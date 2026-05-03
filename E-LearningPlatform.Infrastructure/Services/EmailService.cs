using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;
        public EmailService(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrEmpty(_emailOptions.SmtpHost) || _emailOptions.Port == 0 || string.IsNullOrEmpty(_emailOptions.Username) || string.IsNullOrEmpty(_emailOptions.Password))
            {
                throw new InvalidOperationException("Email configuration is not properly set.");
            }
            var message = new MimeKit.MimeMessage();
            message.From.Add(MailboxAddress.Parse(_emailOptions.From));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_emailOptions.SmtpHost,
                _emailOptions.Port,
                MailKit.Security.SecureSocketOptions.StartTls,
                cancellationToken);
            if (!string.IsNullOrWhiteSpace(_emailOptions.Username))
            {
                await client.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password, cancellationToken);
            }

            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
