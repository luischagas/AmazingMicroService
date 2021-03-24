using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AmazingMicroStore.Core.Application.Messaging.RabbitMQ
{
    public interface IRabbitMqConsumerService
    {
        #region Methods

        EventingBasicConsumer DefineBasicConsumer();

        IModel GetChannel();

        void SetConsumer(EventingBasicConsumer consumer);

        #endregion Methods
    }
}