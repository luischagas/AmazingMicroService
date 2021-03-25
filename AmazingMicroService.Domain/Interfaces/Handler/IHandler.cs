using System.Threading.Tasks;

namespace AmazingMicroService.Domain.Interfaces.Handler
{
    public interface IHandler
    {
        #region Methods

        Task Send<T>(T @event);

        #endregion Methods
    }
}