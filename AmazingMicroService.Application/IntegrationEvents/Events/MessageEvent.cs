using AmazingMicroService.Domain.Interfaces.Events;

namespace AmazingMicroService.Application.IntegrationEvents.Events
{
    public class MessageEvent : Event<MessageEvent>
    {
        #region Properties

        public string Message { get; set; }

        #endregion Properties
    }
}