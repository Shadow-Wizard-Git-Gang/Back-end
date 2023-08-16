using System.Net.Mail;
using System.Net;
using VC.Services.IServices;
using Microsoft.Extensions.Options;
using VC.Data;

namespace VC.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SmtpSettings> _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSetting)
        {
            _smtpSettings = smtpSetting;
        }

        public async Task SendAsync(string from, string to, string subject, string body)
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
    }
}
