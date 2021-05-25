using EventContracts;
using MassTransit;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace EmailHost.Consumers
{
    public class EmailMessageConsumer : IConsumer<EmailMessage>
    {
        readonly ILogger<EmailMessageConsumer> _logger;
        private EmailService _emailService;

        public EmailMessageConsumer(ILogger<EmailMessageConsumer> logger, EmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<EmailMessage> context)
        {
            try
            {
                await _emailService.SendEmailAsync(context.Message);
                _logger.LogInformation("Received message: {head}", context.Message.Subject);
                _logger.LogInformation("Message sent on address: {address}", context.Message.ToEmail);
            }
            catch(Exception ex)
            {
                _logger.LogInformation("An error occured while sending the message: {message}", ex.Message);
            }

        }
    }
}
