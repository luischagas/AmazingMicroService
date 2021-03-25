using AmazingMicroService.Application.IntegrationEvents;
using AmazingMicroService.Application.IntegrationEvents.EventHandlings;
using AmazingMicroService.Application.IntegrationEvents.Events;
using AmazingMicroService.Domain.Interfaces.EventBus.RabbitMQ;
using AmazingMicroService.Domain.Interfaces.EventBus.SubscriptionManager;
using AmazingMicroService.Infrastructure.EventBus.RabbitMQ;
using AmazingMicroService.Infrastructure.EventBus.SubscriptionManager;
using AmazingMicroService.Infrastructure.Handler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using AmazingMicroService.Domain.Interfaces.Handler;

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

            builder.ConfigureServices(services =>
            {
                services.AddMediatR(typeof(MessageEventHandler));

                services.AddScoped<IRequestHandler<MessageEvent>, MessageEventHandler>();
                services.AddScoped<IHandler, Handler>();
                services.AddSingleton<IMemorySubscriptionManager, MemorySubscriptionManager>();
                services.AddSingleton<IMessageIntegrationEventService, MessageIntegrationEventService>();
                services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
                services.AddScoped<MessageEventHandler>();
                services.AddSingleton<IEventBusRabbitMq, EventBusRabbitMq>(serviceProvider =>
                {
                    var integration = new EventBusRabbitMq(serviceProvider, ApplicationName);

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