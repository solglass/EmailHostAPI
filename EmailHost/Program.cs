using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using EmailHost.Consumers;

namespace EmailHost
{
    public class Program
    {
        private static IConfiguration _configuration;
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\services\EmailHostService\Logs\LogFile.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                Log.Information("Starting up the service");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "An Error occured while starting the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureHostConfiguration(configHost =>
            {
                configHost.AddEnvironmentVariables();
                _configuration = configHost.Build();
            }).
            ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<EmailMessageConsumer>();
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.ReceiveEndpoint("EmailMessages", e =>
                        {
                            e.ConfigureConsumer<EmailMessageConsumer>(context);
                        });
                    });
                });
                services.AddMassTransitHostedService();
                services.AddHostedService<Worker>();
                services.AddSingleton<EmailService>();
                services.Configure<SmtpSettings>(_configuration);
            })
            .UseSerilog();
    }
}
