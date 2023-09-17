using Mail.Data;
using MassTransit;
using MessagingContracts;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Mail.Consumers
{
    public class MailConsumer : IConsumer<MessageSent>
    {
        private SmtpSettings _smtpSettings { get; }

        public MailConsumer(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task Consume(ConsumeContext<MessageSent> context)
        {
            var message = new MailMessage(
                context.Message.From,
                context.Message.To,
                context.Message.Header,
                context.Message.Body);


            using (var emailClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                emailClient.Credentials = new NetworkCredential(
                    _smtpSettings.User,
                    _smtpSettings.Password);

                await emailClient.SendMailAsync(message);
            }
        }
    }
}