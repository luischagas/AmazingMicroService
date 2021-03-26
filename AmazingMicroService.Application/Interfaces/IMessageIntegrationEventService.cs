using AmazingMicroService.Domain.Events;
using AmazingMicroService.DomainService.Interfaces.Events;
using System.Threading.Tasks;

namespace AmazingMicroService.Application.Interfaces
{
    public interface IMessageIntegrationEventService
    {
        #region Methods

        Task PublishEventBusAsync(Event<MessageEvent> @event);

        #endregion Methods
    }
}