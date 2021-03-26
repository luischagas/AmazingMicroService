using AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ;
using AmazingMicroService.DomainService.Interfaces.EventBus.SubscriptionManager;
using AmazingMicroService.DomainService.Interfaces.Events;
using AmazingMicroService.DomainService.Interfaces.Handler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AmazingMicroService.Infrastructure.EventBus.RabbitMQ
{
    public class EventBusRabbitMq : IEventBusRabbitMq
    {
        #region Fields

        private readonly IRabbitMqConnection _connection;
        private readonly IMemorySubscriptionManager _memorySubscriptionManager;
        private readonly IHandler _handler;
        private readonly ILogger<EventBusRabbitMq> _logger;

        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly string _exchangeType;
        private readonly string _applicationName;
        private IModel _channel;

        #endregion Fields

        #region Constructors

        public EventBusRabbitMq(IServiceProvider serviceProvider, string applicationName, string queueName, string exchangeName, string exchangeType)
        {
            _queueName = queueName;
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;
            _applicationName = applicationName;

            _connection = serviceProvider.GetRequiredService<IRabbitMqConnection>()
                          ?? throw new ArgumentNullException(nameof(_connection));

            _memorySubscriptionManager = serviceProvider.GetRequiredService<IMemorySubscriptionManager>()
                                         ?? throw new ArgumentNullException(nameof(_memorySubscriptionManager));

            _handler = serviceProvider.GetRequiredService<IHandler>()
                                         ?? throw new ArgumentNullException(nameof(_handler));

            _logger = serviceProvider.GetRequiredService<ILogger<EventBusRabbitMq>>()
                       ?? throw new ArgumentNullException(nameof(_logger));

            _channel = CreateConsumerChannel();
        }

        #endregion Constructors

        #region Methods

        public void Subscribe<TEvent, TRequestHandler>() where TEvent : Event<TEvent> where TRequestHandler : IRequestHandler<TEvent>
        {
            var eventName = typeof(TEvent).Name;

            DoInternalSubscription(eventName);

            _memorySubscriptionManager.AddEventSubscription<TEvent, TRequestHandler>();
        }

        public bool EnqueueEvent<TEvent>(Event<TEvent> @event) where TEvent : Event<TEvent>
        {
            if (_connection.IsConnected is false)
                _connection.TryConnect();

            using var channel = _connection.CreateModel();

            var eventName = @event.GetType().Name;

            channel.ExchangeDeclare(_exchangeName, _exchangeType);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;
            properties.CorrelationId = @event.MicroServiceId;
            try
            {
                _logger.LogInformation("Publishing in queue: {event}", @event);


                channel.BasicPublish(_exchangeName,
                    eventName,
                    true,
                    properties,
                    body);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error publishing in queue: {event}", @event);
                return false;
            }

            return true;
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _memorySubscriptionManager.HasHandlerForEvent(eventName);

            if (containsKey is false)
            {
                using var channel = _connection.CreateModel();

                channel.QueueBind(_queueName, _exchangeName, eventName);
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (_connection.IsConnected is false)
                _connection.TryConnect();

            var channel = _connection.CreateModel();

            channel.ExchangeDeclare(_exchangeName, _exchangeType);

            channel.QueueDeclare(_queueName,
                true,
                false,
                false,
                null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId != _applicationName)
                {
                    var eventName = ea.RoutingKey;

                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    await ProcessEvent(eventName, message);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            channel.BasicConsume(_queueName,
                false,
                consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _channel.Dispose();
                _channel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_memorySubscriptionManager.HasHandlerForEvent(eventName))
            {
                var eventType = _memorySubscriptionManager.GetEventType(eventName);
                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                _logger.LogInformation(
                    "Receiving event ({@IntegrationEvent})",
                     integrationEvent);

                await _handler.Send(integrationEvent);
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _memorySubscriptionManager.Clear();
        }

        #endregion Methods
    }
}