using System;
using MediatR;

namespace AmazingMicroStore.Core.Application.Interfaces.Events
{
    public abstract class Event : INotification
    {
        #region Constructors

        protected Event()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }

        #endregion Constructors

        #region Properties

        public Guid Id { get; set; }
        public string MicroServiceId { get; set; }
        public DateTime Timestamp { get; set; }

        #endregion Properties
    }
}