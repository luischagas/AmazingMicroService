﻿using AmazingMicroService.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingMicroService.Worker
{
    public class Functions : BackgroundService
    {
        #region Fields

        private readonly IMessageIntegrationEventService _messageIntegrationEventService;

        #endregion Fields

        #region Constructors

        public Functions(IMessageIntegrationEventService messageIntegrationEventService)
        {
            _messageIntegrationEventService = messageIntegrationEventService;
        }

        #endregion Constructors

        #region Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($@"Hello, I am the {Startup.ApplicationName}");

            while (stoppingToken.IsCancellationRequested is false)
            {
                await _messageIntegrationEventService.PublishThroughEventBusAsync(Startup.ApplicationName, "Hello World");

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        #endregion Methods
    }
}