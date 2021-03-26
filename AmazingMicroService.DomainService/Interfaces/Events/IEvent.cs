using System;
using MediatR;

namespace AmazingMicroService.DomainService.Interfaces.Events
{
    public interface IEvent : INotification
    {
        #region Properties

        DateTimeOffset TimeStamp { get; }

        #endregion Properties
    }
}