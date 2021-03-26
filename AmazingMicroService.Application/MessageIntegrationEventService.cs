using AmazingMicroService.Application.Interfaces;
using AmazingMicroService.Domain.Events;
using AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ;
using AmazingMicroService.DomainService.Interfaces.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

        public async Task PublishEventBusAsync(Event<MessageEvent> @event)
        {
            try
            {
                _logger.LogInformation("Publishing event: {event}", @event);

                _eventBusRabbitMq.EnqueueEvent(@event);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error publishing event: {event}", @event);
            }

            await Task.Yield();
        }

        #endregion Methods
    }
}