using System.Net.Mail;
using System.Net;
using VC.Services.IServices;
using Microsoft.Extensions.Options;
using VC.Data;
using Microsoft.AspNetCore.Identity;
using VC.Models.Identity;

namespace VC.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration { get; }
        private readonly IOptions<SmtpSettings> _smtpSettings;

        public EmailService(IConfiguration configuration, IOptions<SmtpSettings> smtpSetting)
        {
            _configuration = configuration;
            _smtpSettings = smtpSetting;
        }

        private async Task SendAsync(string from, string to, string subject, string body)
        {
            var message = new MailMessage(from, to, subject, body);

            using (var emailClient = new SmtpClient(_smtpSettings.Value.Host, _smtpSettings.Value.Port))
            {
                emailClient.Credentials = new NetworkCredential(
                    _smtpSettings.Value.User,
                    _smtpSettings.Value.Password);

                await emailClient.SendMailAsync(message);
            }
        }

        public async Task SendConfirmationLetterAsync(string id, string email, string confirmationToken)
        {
            var confirmationLink = string.Format(
                _configuration["URLToTheConfirmationPage"],
                id,
                confirmationToken);

            await SendAsync(
                _configuration["OrganizationEmail"],
                email,
                "Please confirm your email",
                $"Please click on this link to confirm your email address: {confirmationLink}");
        }
    }
}
