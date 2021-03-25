using AmazingMicroService.Application;
using AmazingMicroService.Domain.Events;
using AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ;
using AmazingMicroService.DomainService.Interfaces.EventBus.SubscriptionManager;
using AmazingMicroService.DomainService.Interfaces.Handler;
using AmazingMicroService.Infrastructure.EventBus.RabbitMQ;
using AmazingMicroService.Infrastructure.EventBus.SubscriptionManager;
using AmazingMicroService.Infrastructure.Handler;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using AmazingMicroService.Application.Interfaces;

namespace AmazingMicroService.Worker
{
    public static class Program
    {
        #region Fields

        public static readonly string ApplicationName = $"Service - " + Guid.NewGuid();

        #endregion Fields

        #region Methods

        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            builder.ConfigureServices(services =>
            {
                //var rabbitMqSettings = configuration.GetSection("RabbitMQSettings");

                services.AddMediatR(typeof(MessageEventHandler));

                services.AddScoped<IRequestHandler<MessageEvent>, MessageEventHandler>();
                services.AddScoped<IHandler, Handler>();
                services.AddSingleton<IMemorySubscriptionManager, MemorySubscriptionManager>();
                services.AddSingleton<IMessageIntegrationEventService, MessageIntegrationEventService>();

                services.AddSingleton<IRabbitMqConnection>(i =>
                    new RabbitMqConnection(configuration["Hostname"], configuration["UserName"],
                        configuration["Password"], configuration["RetryAttempts"]));

                services.AddScoped<MessageEventHandler>();
                services.AddSingleton<IEventBusRabbitMq, EventBusRabbitMq>(serviceProvider =>
                {
                    var integration = new EventBusRabbitMq(serviceProvider, ApplicationName, configuration["QueueName"], configuration["ExchangeName"], configuration["ExchangeType"]);

                    integration.Subscribe<MessageEvent, MessageEventHandler>();

                    return integration;
                });

                services.AddHostedService<Functions>();
            });

            var host = builder.UseConsoleLifetime().Build();

            using (host)
            {
                await host.RunAsync();
            }
        }

        #endregion Methods
    }
}