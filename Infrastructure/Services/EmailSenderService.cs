using Core.Interfaces.ExternalServices;
using Infrastructure.Services.Configs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailSenderConfig _config;
        private readonly IHostEnvironment _environment;

        public EmailSenderService(IOptions<EmailSenderConfig> options, IHostEnvironment environment)
        {
            _config = options.Value;
            _environment = environment;
        }

        public async Task<bool> SendVrfCode(string toEmail, string vrfCode)
        {
            var subject = "Confirm your registration!";
            var message = $"<h2>Your verification code: <strong>{vrfCode}</strong>.</h2>";

            return await SendEmailAsync(toEmail, subject, message);
        }

        public async Task<bool> SendPasswordRecoverLink(string toEmail, string token)
        {
            var baseUrl = _environment.IsDevelopment()
                ? "http://localhost:5173"
                : "https://healthsync-client.up.railway.app";

            var subject = "Password recover!";
            var message = $"<a href='{baseUrl}/account/recoverPassword?token={token}' style='display: inline-block; padding: 10px 20px; background-color: #01bfa5; color: white; text-decoration: none; border-radius: 0.75rem; font-size: 16px;'>Recover Password</a>";

            return await SendEmailAsync(toEmail, subject, message);
        }

        /// <summary>
        /// This method send emails.
        /// </summary>
        private async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.From));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = htmlMessage };

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_config.SmtpServer, _config.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config.Username, _config.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
