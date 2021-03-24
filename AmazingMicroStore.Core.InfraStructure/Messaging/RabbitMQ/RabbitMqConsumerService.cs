using System;
using AmazingMicroStore.Core.Application.Messaging.RabbitMQ;
using AmazingMicroStore.Core.InfraStructure.Properties;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AmazingMicroStore.Core.InfraStructure.Messaging.RabbitMQ
{
    public class RabbitMqConsumerService : IRabbitMqConsumerService
    {
        #region Fields

        private readonly IModel _channel;

        #endregion Fields

        #region Constructors

        public RabbitMqConsumerService()
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

        public EventingBasicConsumer DefineBasicConsumer()
        {
            return new EventingBasicConsumer(_channel);
        }

        public IModel GetChannel()
        {
            return _channel;
        }

        public void SetConsumer(EventingBasicConsumer consumer)
        {
            _channel.BasicQos(0, 1, false);

            _channel.BasicConsume(
                queue: Resources.QueueName,
                autoAck: false,
                consumer: consumer
            );
        }

        #endregion Methods
    }
}