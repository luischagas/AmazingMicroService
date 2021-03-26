using AmazingMicroService.Application;
using AmazingMicroService.Application.Interfaces;
using AmazingMicroService.Domain.Events;
using AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ;
using AmazingMicroService.DomainService.Interfaces.EventBus.SubscriptionManager;
using AmazingMicroService.DomainService.Interfaces.Handler;
using AmazingMicroService.Infrastructure.EventBus.RabbitMQ;
using AmazingMicroService.Infrastructure.EventBus.SubscriptionManager;
using AmazingMicroService.Infrastructure.Handler;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;
using System;
using System.IO;

namespace AmazingMicroService.Worker
{
    public class Startup
    {
        #region Fields

        public static readonly string ApplicationName = $"Service - " + Guid.NewGuid();

        #endregion Fields

        #region Constructors

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        #endregion Constructors

        #region Properties

        private IConfigurationRoot Configuration { get; }

        #endregion Properties

        #region Methods

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            ConfigureSerilog(services);

            services.AddMediatR(typeof(MessageEventHandler));

            services.AddScoped<IRequestHandler<MessageEvent>, MessageEventHandler>();
            services.AddScoped<IHandler, Handler>();
            services.AddSingleton<IMemorySubscriptionManager, MemorySubscriptionManager>();
            services.AddSingleton<IMessageIntegrationEventService, MessageIntegrationEventService>();

            services.AddSingleton<IRabbitMqConnection>(i =>
                new RabbitMqConnection(Configuration["RabbitMqHost"], Configuration["RabbitMqUser"],
                    Configuration["RabbitMqPassword"], Configuration["RabbitMqRetryAttempts"]));

            services.AddScoped<MessageEventHandler>();
            services.AddSingleton<IEventBusRabbitMq, EventBusRabbitMq>(serviceProvider =>
            {
                var integration = new EventBusRabbitMq(serviceProvider, ApplicationName, Configuration["RabbitMqQueueName"], Configuration["RabbitMqExchangeName"], Configuration["RabbitMqExchangeType"]);

                integration.Subscribe<MessageEvent, MessageEventHandler>();

                return integration;
            });

            services.AddHostedService<Functions>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
        }

        private void ConfigureSerilog(IServiceCollection services)
        {
            var serilogConfiguration = Configuration.GetSection("Serilog");

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