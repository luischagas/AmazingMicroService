using AmazingMicroService.DomainService.Interfaces.Events;
using MediatR;
using System;

namespace AmazingMicroService.DomainService.Interfaces.EventBus.SubscriptionManager
{
    public interface IMemorySubscriptionManager
    {
        #region Methods

        void AddEventSubscription<TEvent, TRequestHandler>()
            where TEvent : Event<TEvent>
            where TRequestHandler : IRequestHandler<TEvent>;

        Type GetEventType(string eventName);

        bool HasHandlerForEvent(string eventName);

        void Clear();

        #endregion Methods
    }
}