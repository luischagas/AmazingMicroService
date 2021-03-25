using AmazingMicroService.Domain.Interfaces.EventBus.RabbitMQ;
using System.Threading.Tasks;
using AmazingMicroService.Domain.Interfaces.Events;

namespace AmazingMicroService.Application.IntegrationEvents
{
    public class MessageIntegrationEventService : IMessageIntegrationEventService
    {
        #region Fields

        private readonly IEventBusRabbitMq _integrationRabbitMqService;

        #endregion Fields

        #region Constructors

        public MessageIntegrationEventService(IEventBusRabbitMq integrationRabbitMqService)
        {
            _integrationRabbitMqService = integrationRabbitMqService;
        }

        #endregion Constructors

        #region Methods

        public async Task PublishThroughEventBusAsync<T>(T @event) where T : Event<T>
        {
            _integrationRabbitMqService.EnqueueEvent(@event);
        }

        #endregion Methods
    }
}