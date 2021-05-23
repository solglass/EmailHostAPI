using System.Threading.Tasks;
using EventContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EmailHost.Consumers
{
    public class SetupInfoConsumer : IConsumer<SetupCodeInfo>
    {
        readonly ILogger<SetupInfoConsumer> _logger;
        private EmailService _emailService;

        public SetupInfoConsumer(ILogger<SetupInfoConsumer> logger, EmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<SetupCodeInfo> context)
        {
            _logger.LogInformation("Manual GA Code for email recieved: ");
            await _emailService.SendEmailWithImageAsync(context.Message);
        }
    }
}
