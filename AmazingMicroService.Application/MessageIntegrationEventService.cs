using System;
using AmazingMicroService.Domain.Events;
using AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ;
using System.Threading.Tasks;
using AmazingMicroService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AmazingMicroService.Application
{
    public class MessageIntegrationEventService : IMessageIntegrationEventService
    {
        #region Fields

        private readonly IEventBusRabbitMq _eventBusRabbitMq;
        private readonly ILogger<MessageIntegrationEventService> _logger;

        #endregion Fields

        #region Constructors

        public MessageIntegrationEventService(IEventBusRabbitMq eventBusRabbitMq, ILogger<MessageIntegrationEventService> logger)
        {
            _eventBusRabbitMq = eventBusRabbitMq;
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public async Task PublishThroughEventBusAsync(string applicationName, string message)
        {
            var @event = new MessageEvent()
            {
                MicroServiceId = applicationName,
                Message = message
            };

            try
            {
                _logger.LogInformation("Publishing event: {event}", @event);

                _eventBusRabbitMq.EnqueueEvent(@event);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Error publishing event: {event}", @event);
            }

        }

        #endregion Methods
    }
}