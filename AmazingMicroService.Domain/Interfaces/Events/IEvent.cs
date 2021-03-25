using MediatR;
using System;

namespace AmazingMicroService.Domain.Interfaces.Events
{
    public interface IEvent : INotification
    {
        #region Properties

        DateTimeOffset TimeStamp { get; }

        #endregion Properties
    }
}