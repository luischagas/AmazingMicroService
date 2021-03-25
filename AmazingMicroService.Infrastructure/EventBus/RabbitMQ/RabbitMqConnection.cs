using AmazingMicroService.Domain.Interfaces.EventBus.RabbitMQ;
using AmazingMicroService.Infrastructure.Properties;
using RabbitMQ.Client;
using System;

namespace AmazingMicroService.Infrastructure.EventBus.RabbitMQ
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        #region Fields

        private readonly IConnectionFactory _connectionFactory;
        private readonly object _connectionLock = new object();
        private IConnection _connection;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public RabbitMqConnection()
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = Resources.Hostname,
                UserName = Resources.UserName,
                Password = Resources.Password
            };
        }

        #endregion Constructors

        #region Properties

        public bool IsConnected => _connection != null && _connection.IsOpen && _disposed is false;

        #endregion Properties

        #region Methods

        public IModel CreateModel()
        {
            if (IsConnected is false)
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");

            return _connection.CreateModel();
        }

        public bool TryConnect()
        {
            lock (_connectionLock)
            {
                _connection = _connectionFactory
                    .CreateConnection();

                return IsConnected;
            }
        }

        public void Dispose()
        {
            if (_disposed is false)
                return;

            _disposed = true;
            _connection.Dispose();
        }

        #endregion Methods
    }
}