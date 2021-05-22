using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventContracts;
using MassTransit;
using Newtonsoft.Json.Linq;
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
            _logger.LogInformation("Received Text: {Text}", context.Message.Value);
            context.Message.Value.TryGetValue("Account", out string email);
            context.Message.Value.TryGetValue("ManualEntryKey", out string manualEntryKey);
            context.Message.Value.TryGetValue("QrCodeSetupImageUrl", out string qrCode);
            await _emailService.SendEmailAsync("yellowcucumber27@gmail.com", "An Error Occured", manualEntryKey);
        }
    }
}
