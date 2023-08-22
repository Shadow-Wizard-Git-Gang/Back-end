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
        private const int _delayBeforeSendingLetterInSeconds = 20;

        private IConfiguration _configuration { get; }
        private readonly IOptions<SmtpSettings> _smtpSettings;
        private readonly ILogger _logger;

        public EmailService(IConfiguration configuration, IOptions<SmtpSettings> smtpSetting, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _smtpSettings = smtpSetting;
            _logger = logger;
        }

        private async Task SendAsync(MailMessage message)
        {
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

            var message = new MailMessage(
                _configuration["OrganizationEmail"], 
                email, 
                "Please confirm your email",
                $"Please click on this link to confirm your email address: {confirmationLink}");

            try
            {
                await SendAsync(message);
            }
            catch
            {
                await Task.Delay(_delayBeforeSendingLetterInSeconds * 1000);

                try
                {
                    await SendAsync(message);
                }
                catch(Exception ex) {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}
