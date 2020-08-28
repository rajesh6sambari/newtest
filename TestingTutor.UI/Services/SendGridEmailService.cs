using System.Threading.Tasks;
using TestingTutor.Dev.Engine;
using TestingTutor.Dev.Engine.Data;
using TestingTutor.UI.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TestingTutor.UI.Services
{
    public class SendGridEmailService : IEmailService
    {
        public SendGridOptions Options;

        public SendGridEmailService(IOptions<SendGridOptions> options)
        {
            Options = options.Value;
        }

        public async Task Send(EmailData data)
        {
            var client = new SendGridClient(Options.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.Email, Options.Name),
                Subject = data.Subject,
                PlainTextContent = data.Content,
            };
            msg.AddTo(new EmailAddress(data.Email, data.Name));
            await client.SendEmailAsync(msg);
        }
    }
}
