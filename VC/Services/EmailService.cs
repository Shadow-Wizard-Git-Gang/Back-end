using VC.Services.IServices;
using MassTransit;
using MessagingContracts;

namespace VC.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration { get; }
        private IPublishEndpoint _publishEndpoint { get; }

        public EmailService(IConfiguration configuration, IPublishEndpoint publishEndpoint)
        {
            _configuration = configuration;
            _publishEndpoint = publishEndpoint;
        }

        public async Task SendConfirmationLetterAsync(string email, string id, string confirmationToken)
        {
            var confirmationLink = string.Format(
                _configuration["URLToTheConfirmationPage"],
                id,
                confirmationToken);

            var message = new MessageSent(
                _configuration["OrganizationEmail"], 
                email, 
                "Please confirm your email",
                $"Please click on this link to confirm your email address: {confirmationLink}");
           
            await _publishEndpoint.Publish(message);
        }

        public async Task SendPasswordResettingLetterAsync(string email, string id, string resettingToken)
        {
            var resettingLink = string.Format(
                    _configuration["URLToTheResettingPasswordPage"],
                    id,
                    resettingToken
                );

            var message = new MessageSent(
                _configuration["OrganizationEmail"],
                email,
                "Please reset your password",
                $"Please click on this link to reset your password: {resettingLink}");

            await _publishEndpoint.Publish(message);
        }
    }
}
