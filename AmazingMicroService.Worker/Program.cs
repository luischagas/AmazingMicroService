﻿using AmazingMicroService.Application;
using AmazingMicroService.Application.Interfaces;
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
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AmazingMicroService.Worker
{
    public static class Program
    {
        #region Fields

        public static readonly string ApplicationName = $"Service - " + Guid.NewGuid();
        private static IConfigurationRoot _configuration;

        #endregion Fields

        #region Methods

        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder();

            var environmentConfiguration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var environmentName = environmentConfiguration["ASPNETCORE_ENVIRONMENT"];

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, false)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            builder
                .ConfigureServices(services =>
            {
                var rabbitMqSettings = _configuration.GetSection("RabbitMQSettings");

                ConfigureSerilog(services);

                services.AddMediatR(typeof(MessageEventHandler));

                services.AddScoped<IRequestHandler<MessageEvent>, MessageEventHandler>();
                services.AddScoped<IHandler, Handler>();
                services.AddSingleton<IMemorySubscriptionManager, MemorySubscriptionManager>();
                services.AddSingleton<IMessageIntegrationEventService, MessageIntegrationEventService>();

                services.AddSingleton<IRabbitMqConnection>(i =>
                    new RabbitMqConnection(rabbitMqSettings["RabbitMqHost"], rabbitMqSettings["RabbitMqUser"],
                        rabbitMqSettings["RabbitMqPassword"], rabbitMqSettings["RabbitMqRetryAttempts"]));

                services.AddScoped<MessageEventHandler>();
                services.AddSingleton<IEventBusRabbitMq, EventBusRabbitMq>(serviceProvider =>
                {
                    var integration = new EventBusRabbitMq(serviceProvider, ApplicationName, rabbitMqSettings["RabbitMqQueueName"], rabbitMqSettings["RabbitMqExchangeName"], rabbitMqSettings["RabbitMqExchangeType"]);

                    integration.Subscribe<MessageEvent, MessageEventHandler>();

                    return integration;
                });

                services.AddHostedService<Functions>();
            });

            var host = builder
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                await host.RunAsync();
            }
        }

        private static void ConfigureSerilog(IServiceCollection services)
        {
            var serilogConfiguration = _configuration.GetSection("Serilog");

            var rabbitConfig = new RabbitMQClientConfiguration
            {
                Username = serilogConfiguration["RabbitMqUser"],
                Password = serilogConfiguration["RabbitMqPassword"],
                Exchange = "",
                RouteKey = serilogConfiguration["RabbitMqRoutingKey"],
                Port = Convert.ToInt32(serilogConfiguration["RabbitMqPort"]),
                DeliveryMode = RabbitMQDeliveryMode.Durable,
            };

            rabbitConfig.Hostnames.Add(serilogConfiguration["RabbitMqHost"]);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", new LoggingLevelSwitch(LogEventLevel.Error))
                .Enrich.FromLogContext()
                //.WriteTo.Console() If you want to view the log on the console
                .WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
                {
                    clientConfiguration.From(rabbitConfig);
                    sinkConfiguration.TextFormatter = new JsonFormatter();
                })
                .CreateLogger();

            var loggerFactory = new LoggerFactory();

            loggerFactory.AddSerilog();

            services.AddSingleton<ILoggerFactory>(loggerFactory);
        }

        #endregion Methods
    }
}