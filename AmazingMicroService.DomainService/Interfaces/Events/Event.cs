using MediatR;
using System;

namespace AmazingMicroService.DomainService.Interfaces.Events
{
    public abstract class Event<T> : IRequest, IEvent where T : Event<T>
    {
        #region Constructors

        protected Event()
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
        }

        #endregion Constructors

        #region Properties

        public Guid Id { get; }

        public string MicroServiceId { get; set; }
        public DateTimeOffset TimeStamp { get; }

        #endregion Properties
    }
}