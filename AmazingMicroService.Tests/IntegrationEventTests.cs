namespace AmazingMicroService.Tests
{
    public class IntegrationEventTests
    {
        //#region Fields

        //private readonly Mock<IEventBusRabbitMq> _eventBusRabbitMqFaker;
        //private readonly Mock<IEvent> _event;
        //private readonly Mock<ILogger<Mock>> _logger;

        //#endregion Fields

        //#region Constructors

        //public IntegrationEventTests()
        //{
        //    _eventBusRabbitMqFaker = new Mock<IEventBusRabbitMq>();
        //    _event = new Mock<IEvent>();
        //    _logger = new Mock<ILogger<Mock>>();
        //}

        //#endregion Constructors

        //#region Methods

        //[Fact(DisplayName = "Given a valid event, it must be published in the queue.")]
        //public async Task PublishHelloWordMessageEvent()
        //{
        //    var service = new MessageIntegrationEventService(_eventBusRabbitMqFaker.Object, _logger.Object);

        //    await service.PublishThroughEventBusAsync($"Service - {Guid.NewGuid()}", "Hello World");

        //    var @event = new MessageEvent()
        //    {
        //        MicroServiceId = $"Service - {Guid.NewGuid()}",
        //        Message = "Hello World"
        //    };

        //    _eventBusRabbitMqFaker.Verify(x => x.EnqueueEvent(@event));
        //}

        //#endregion Methods
    }
}