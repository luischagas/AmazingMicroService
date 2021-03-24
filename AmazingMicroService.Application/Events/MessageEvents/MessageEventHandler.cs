using System.Text.Json.Serialization;
using AmazingMicroService.Application.Events.MessageEvents.Input;
using AmazingMicroStore.Core.Application.Messaging.RabbitMQ;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AmazingMicroService.Application.Events.MessageEvents
{
    public class MessageEventHandler :
        INotificationHandler<SendMessageEvent>

    {
        #region Fields

        private readonly IRabbitMqPublisherService _rabbitMqPublisherService;

        #endregion Fields

        #region Constructors

        public MessageEventHandler(IRabbitMqPublisherService rabbitMqPublisherService)
        {
            _rabbitMqPublisherService = rabbitMqPublisherService;
        }

        #endregion Constructors

        #region Methods

        public Task Handle(SendMessageEvent notification, CancellationToken cancellationToken)
        {
            _rabbitMqPublisherService.EnqueueMessage(JsonConvert.SerializeObject(notification));

            return Task.CompletedTask;
        }

        #endregion Methods
    }
}