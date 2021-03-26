using AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;

namespace AmazingMicroService.Infrastructure.EventBus.RabbitMQ
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        #region Fields

        private readonly IConnectionFactory _connectionFactory;
        private readonly object _connectionLock = new object();
        private readonly int _retryAttempts;
        private IConnection _connection;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public RabbitMqConnection(string hostname, string username, string password, string retryAttempts)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = hostname,
                UserName = username,
                Password = password
            };

            _retryAttempts = int.Parse(retryAttempts);
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
                var policy = Policy
                    .Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryAttempts, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                policy.Execute(() => _connection = _connectionFactory.CreateConnection());

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