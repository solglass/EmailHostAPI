using EventContracts;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace EmailHost
{
    public class EmailService
    {
        private SmtpSettings _smtpSettings;
        public EmailService(IOptions<SmtpSettings> options)
        {
            _smtpSettings = options.Value;
        }
        public async Task SendEmailAsync(EmailMessage message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", _smtpSettings.EMAILSERVICE_ADMIN_EMAIL_ADDRESS));
            emailMessage.To.Add(new MailboxAddress(message.ToName, message.ToEmail));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.EMAILSERVICE_SMTPSERVER_ADDRESS, _smtpSettings.EMAILSERVICE_SMTPPORT, true);
                await client.AuthenticateAsync(_smtpSettings.EMAILSERVICE_ADMIN_LOGIN, _smtpSettings.EMAILSERVICE_ADMIN_PASSWORD);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
