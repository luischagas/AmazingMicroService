using AmazingMicroService.Domain.Events;
using Moq;
using System;
using System.Threading.Tasks;
using AmazingMicroService.Application;
using AmazingMicroService.DomainService;
using AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ;
using Xunit;

namespace AmazingMicroService.Tests
{
    public class IntegrationEventTests
    {
        #region Fields

        private readonly Mock<IEventBusRabbitMq> _eventBusRabbitMqFaker;

        #endregion Fields

        #region Constructors

        public IntegrationEventTests()
        {
            _eventBusRabbitMqFaker = new Mock<IEventBusRabbitMq>();
        }

        #endregion Constructors

        #region Methods

        [Fact(DisplayName = "Given a valid event, it must be published in the queue.")]
        public async Task PublishHelloWordMessageEvent()
        {
            var service = new MessageIntegrationEventService(_eventBusRabbitMqFaker.Object);

            await service.PublishThroughEventBusAsync($"Service - {Guid.NewGuid()}", "Hello World");

            var @event = new MessageEvent()
            {
                MicroServiceId = $"Service - {Guid.NewGuid()}",
                Message = "Hello World"
            };

            _eventBusRabbitMqFaker.Verify(x => x.EnqueueEvent(@event));
        }

        #endregion Methods
    }
}