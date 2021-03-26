using AmazingMicroService.DomainService.Interfaces.EventBus.SubscriptionManager;
using AmazingMicroService.DomainService.Interfaces.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using AmazingMicroService.Infrastructure.EventBus.SubscriptionManager.Models;

namespace AmazingMicroService.Infrastructure.EventBus.SubscriptionManager
{
    public class MemorySubscriptionManager : IMemorySubscriptionManager
    {
        #region Fields

        private readonly Dictionary<string, List<Subscription>> _subscriptions;

        private readonly List<Type> _eventTypes;

        #endregion Fields

        #region Constructors

        public MemorySubscriptionManager()
        {
            _subscriptions = new Dictionary<string, List<Subscription>>();
            _eventTypes = new List<Type>();
        }

        #endregion Constructors

        #region Methods

        public void AddEventSubscription<TEvent, TRequestHandler>() where TEvent : Event<TEvent> where TRequestHandler : IRequestHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();

            DoAddSubscription(typeof(TRequestHandler), eventName);

            if (_eventTypes.Contains(typeof(TEvent)) is false)
                _eventTypes.Add(typeof(TEvent));
        }

        public Type GetEventType(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public bool HasHandlerForEvent(string eventName) => _subscriptions.ContainsKey(eventName);

        public void Clear() => _subscriptions.Clear();

        public string GetEventKey<T>() => typeof(T).Name;

        private void DoAddSubscription(Type handlerType, string eventName)
        {
            if (HasHandlerForEvent(eventName) is false)
                _subscriptions.Add(eventName, new List<Subscription>());

            if (_subscriptions[eventName].Any(s => s.HandlerType == handlerType))
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'",
                    nameof(handlerType));

            _subscriptions[eventName].Add(new Subscription(handlerType));
        }

        #endregion Methods
    }
}