using System.Threading.Tasks;
using AmazingMicroStore.Core.Application.Interfaces.Events;

namespace AmazingMicroStore.Core.Application.Interfaces.Handler
{
    public interface IHandler
    {
        #region Methods

        /// <summary>
        /// Handle an Event
        /// </summary>
        /// <returns></returns>
        Task RaiseEvent<T>(T @event) where T : Event;

        #endregion Methods
    }
}