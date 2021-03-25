using RabbitMQ.Client;
using System;

namespace AmazingMicroService.Domain.Interfaces.EventBus.RabbitMQ
{
    public interface IRabbitMqConnection : IDisposable
    {
        #region Properties

        bool IsConnected { get; }

        #endregion Properties

        #region Methods

        IModel CreateModel();

        bool TryConnect();

        #endregion Methods
    }
}