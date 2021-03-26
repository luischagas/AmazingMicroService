using AmazingMicroService.Application;
using AmazingMicroService.Domain.Events;
using AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AmazingMicroService.Tests
{
    public class IntegrationEventTests
    {
        #region Fields

        private readonly Mock<IEventBusRabbitMq> _eventBusRabbitMq;
        private readonly Mock<ILogger<MessageIntegrationEventService>> _logger;

        #endregion Fields

        #region Constructors

        public IntegrationEventTests()
        {
            _eventBusRabbitMq = new Mock<IEventBusRabbitMq>();
            _logger = new Mock<ILogger<MessageIntegrationEventService>>();
        }

        #endregion Constructors

        #region Methods

        [Fact(DisplayName = "Given a valid event, it must be published in the queue.")]
        public async Task PublishHelloWordMessageEvent()
        {
            var service = new MessageIntegrationEventService(_eventBusRabbitMq.Object, _logger.Object);

            var @event = new MessageEvent()
            {
                MicroServiceId = $"Service - {Guid.NewGuid()}",
                Message = "Hello World"
            };

            await service.PublishEventBusAsync(@event);

            _eventBusRabbitMq.Verify(x => x.EnqueueEvent(@event));
        }

        #endregion Methods
    }
}