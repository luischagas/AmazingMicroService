﻿using AmazingMicroService.Application.Interfaces;
using AmazingMicroService.Domain.Events;
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
            Console.WriteLine($@"Hello, I am the {Program.ApplicationName}");

            while (stoppingToken.IsCancellationRequested is false)
            {
                await _messageIntegrationEventService.PublishEventBusAsync(new MessageEvent()
                {
                    Message = "Hello World",
                    MicroServiceId = Program.ApplicationName
                });

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        #endregion Methods
    }
}