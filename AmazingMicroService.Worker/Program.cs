using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using AmazingMicroService.Application.Events.MessageEvents;
using AmazingMicroService.Application.Events.MessageEvents.Input;
using AmazingMicroStore.Core.Application.Interfaces.Handler;
using AmazingMicroStore.Core.Application.Messaging.RabbitMQ;
using AmazingMicroStore.Core.InfraStructure.Handler;
using AmazingMicroStore.Core.InfraStructure.Messaging.RabbitMQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
                services.AddSingleton<IRabbitMqPublisherService, RabbitMqPublisherService>();
                services.AddSingleton<IRabbitMqConsumerService, RabbitMqConsumerService>();
                services.AddScoped<INotificationHandler<SendMessageEvent>, MessageEventHandler>();
                services.AddScoped<IHandler, Handler>();
                services.AddHostedService<Functions>();
                services.AddMediatR(AppDomain.CurrentDomain.Load("AmazingMicroService.Worker"));
            });

            var host = builder.Build();

            using (host)
            {
                await host.RunAsync();
            }
        }

        #endregion Methods
    }
}