using AmazingMicroService.Domain.Events;
using AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ;
using System.Threading.Tasks;
using AmazingMicroService.Application.Interfaces;

namespace AmazingMicroService.Application
{
    public class MessageIntegrationEventService : IMessageIntegrationEventService
    {
        #region Fields

        private readonly IEventBusRabbitMq _eventBusRabbitMq;

        #endregion Fields

        #region Constructors

        public MessageIntegrationEventService(IEventBusRabbitMq eventBusRabbitMq)
        {
            _eventBusRabbitMq = eventBusRabbitMq;
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

            _eventBusRabbitMq.EnqueueEvent(@event);
        }

        #endregion Methods
    }
}