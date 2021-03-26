using AmazingMicroService.DomainService.Interfaces.Events;
using MediatR;
using System;

namespace AmazingMicroService.DomainService.Interfaces.EventBus.RabbitMQ
{
    public interface IEventBusRabbitMq : IDisposable
    {
        #region Methods

        void Subscribe<TEvent, TRequestHandler>() where TEvent : Event<TEvent>
            where TRequestHandler : IRequestHandler<TEvent>;

        void EnqueueEvent<TEvent>(Event<TEvent> @event) where TEvent : Event<TEvent>;

        #endregion Methods
    }
}