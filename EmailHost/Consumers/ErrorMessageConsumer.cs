using EventContracts;
using MassTransit;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace EmailHost.Consumers
{
    public class ErrorMessageConsumer : IConsumer<ErrorMessage>
    {
        readonly ILogger<ErrorMessageConsumer> _logger;
        private EmailService _emailService;

        public ErrorMessageConsumer(ILogger<ErrorMessageConsumer> logger, EmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<ErrorMessage> context)
        {
            try
            {
                _logger.LogInformation("Received Text: {Text}", context.Message.Value);
                await _emailService.SendEmailAsync("educationsystem42@gmail.com", "An Error Occured", context.Message.Value);
                _logger.LogInformation("Message sent on address: {address}", "educationsystem42@gmail.com");
            }
            catch
            {
                _logger.LogInformation("An error occured while sending the message");
            }

        }
    }
}
