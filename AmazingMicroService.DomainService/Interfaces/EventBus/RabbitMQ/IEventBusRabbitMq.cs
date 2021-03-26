using System;
using AmazingMicroService.DomainService.Interfaces.Events;
using MediatR;

namespace AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ
{
    public interface IEventBusRabbitMq : IDisposable
    {
        #region Methods

        void Subscribe<TEvent, TRequestHandler>() where TEvent : Event<TEvent>
            where TRequestHandler : IRequestHandler<TEvent>;

        bool EnqueueEvent<TEvent>(Event<TEvent> @event) where TEvent : Event<TEvent>;

        #endregion Methods
    }
}