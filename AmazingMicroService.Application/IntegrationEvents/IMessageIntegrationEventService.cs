using System.Threading.Tasks;
using AmazingMicroService.Domain.Interfaces.Events;

namespace AmazingMicroService.Application.IntegrationEvents
{
    public interface IMessageIntegrationEventService
    {
        #region Methods

        Task PublishThroughEventBusAsync<T>(T @event) where T : Event<T>;

        #endregion Methods
    }
}