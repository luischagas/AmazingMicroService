using System;
using System.Text;
using AmazingMicroStore.Core.Application.Messaging.RabbitMQ;
using AmazingMicroStore.Core.InfraStructure.Properties;
using RabbitMQ.Client;

namespace AmazingMicroStore.Core.InfraStructure.Messaging.RabbitMQ
{
    public class RabbitMqPublisherService : IRabbitMqPublisherService
    {
        #region Fields

        private readonly IModel _channel;

        #endregion Fields

        #region Constructors

        public RabbitMqPublisherService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = Resources.HostName,
                Port = int.Parse(Resources.Port),
                UserName = Resources.UserName,
                Password = Resources.Password,
            };

            try
            {
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();

                RabbitMqInitializer.Initiate(_channel);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion Constructors

        #region Methods

        public bool EnqueueMessage(string message)
        {
            var bodyMessage = Encoding.UTF8.GetBytes(message);

            try
            {
                IBasicProperties props = _channel.CreateBasicProperties();
                props.Persistent = true;

                _channel.BasicPublish(
                    exchange: Resources.ExchangeName,
                    routingKey: Resources.RoutingKey,
                    basicProperties: props,
                    body: bodyMessage
                );
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion Methods
    }
}