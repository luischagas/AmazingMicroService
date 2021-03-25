using AmazingMicroService.DomainService.Interfaces.Events;

namespace AmazingMicroService.Domain.Events
{
    public class MessageEvent : Event<MessageEvent>
    {
        #region Properties

        public string Message { get; set; }

        #endregion Properties
    }
}