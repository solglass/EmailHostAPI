using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.Configuration;
using EmailHost.Consumers;

namespace EmailHost
{
    public class Program
    {
        private static IConfiguration _configuration;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
            ConfigureHostConfiguration(configHost =>
            {
                configHost.AddJsonFile("appsettings.json");
                _configuration = configHost.Build();
            }).
            ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<SetupInfoConsumer>();
                    x.AddConsumer<ErrorMessageConsumer>();
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.ReceiveEndpoint("ratesApi-error-listener", e =>
                        {
                            e.ConfigureConsumer<ErrorMessageConsumer>(context);
                        });
                        cfg.ReceiveEndpoint("setupInfo", e =>
                        {
                            e.ConfigureConsumer<SetupInfoConsumer>(context);
                        });
                    });
                });
                services.AddMassTransitHostedService();
                services.AddHostedService<Worker>();
                services.AddSingleton<EmailService>();
                services.Configure<SmtpSettings>(_configuration);
            });
    }
}
