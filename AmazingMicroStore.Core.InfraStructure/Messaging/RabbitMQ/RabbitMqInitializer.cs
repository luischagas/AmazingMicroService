using AmazingMicroStore.Core.InfraStructure.Properties;
using RabbitMQ.Client;

namespace AmazingMicroStore.Core.InfraStructure.Messaging.RabbitMQ
{
    public static class RabbitMqInitializer
    {
        #region Methods

        public static void Initiate(IModel channel)
        {
            channel.QueueDeclare(
                queue: Resources.QueueName,
                durable: true,
                exclusive: false,
                autoDelete:false,
                arguments: null
            );

            channel.ExchangeDeclare(
                exchange: Resources.ExchangeName,
                type: Resources.ExchangeType,
                durable: true,
                autoDelete: false,
                arguments: null
            );

            channel.QueueBind(
                queue: Resources.QueueName,
                exchange: Resources.ExchangeName,
                routingKey: Resources.RoutingKey,
                arguments: null
            );
        }

        #endregion Methods
    }
}