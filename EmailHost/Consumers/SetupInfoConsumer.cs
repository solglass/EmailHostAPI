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
            _logger.LogInformation("Received Text: {Text}", context.Message.SendValue);
            context.Message.SendValue.TryGetValue("Account", out string email);
            context.Message.SendValue.TryGetValue("ManualEntryKey", out string manualEntryKey);
            context.Message.SendValue.TryGetValue("QrCodeSetupImageUrl", out string qrCode);
            await _emailService.SendEmailAsync("spbahls@gmail.com", "ManualEntryKey", manualEntryKey);
        }
    }
}
