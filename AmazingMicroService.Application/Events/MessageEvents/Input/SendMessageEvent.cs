using AmazingMicroStore.Core.Application.Interfaces.Events;

namespace AmazingMicroService.Application.Events.MessageEvents.Input
{
    public class SendMessageEvent : Event
    {
        #region Properties

        public string Message { get; set; }


        #endregion Properties
    }
}