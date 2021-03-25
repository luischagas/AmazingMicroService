using System.Threading.Tasks;

namespace AmazingMicroService.Application.Interfaces
{
    public interface IMessageIntegrationEventService
    {
        #region Methods

        Task PublishThroughEventBusAsync(string applicationName, string message);

        #endregion Methods
    }
}