using AmazingMicroService.Application.IntegrationEvents;
using AmazingMicroService.Application.IntegrationEvents.Events;
using AmazingMicroService.Domain.Interfaces.EventBus.RabbitMQ;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AmazingMicroService.Tests
{
    public class IntegrationEventTests
    {
        #region Fields

        private readonly Mock<IEventBusRabbitMq> _integrationRabbitMqServiceFaker;

        #endregion Fields

        #region Constructors

        public IntegrationEventTests()
        {
            _integrationRabbitMqServiceFaker = new Mock<IEventBusRabbitMq>();
        }

        #endregion Constructors

        #region Methods

        [Fact(DisplayName = "Given a valid event, it must be published in the queue.")]
        public async Task PublishHelloWordMessageEvent()
        {
            var @event = new MessageEvent()
            {
                MicroServiceId = $"Service - {Guid.NewGuid()}",
                Message = "Hello World"
            };

            var service = new MessageIntegrationEventService(_integrationRabbitMqServiceFaker.Object);

            await service.PublishThroughEventBusAsync(@event);

            _integrationRabbitMqServiceFaker.Verify(x => x.EnqueueEvent(@event));
        }

        #endregion Methods
    }
}