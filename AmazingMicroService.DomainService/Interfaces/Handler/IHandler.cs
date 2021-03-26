using System.Threading.Tasks;

namespace AmazingMicroService.DomainService.Interfaces.Handler
{
    public interface IHandler
    {
        #region Methods

        Task Send<T>(T @event);

        #endregion Methods
    }
}