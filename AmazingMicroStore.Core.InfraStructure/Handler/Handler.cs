using System.Threading.Tasks;
using AmazingMicroStore.Core.Application.Interfaces.Events;
using AmazingMicroStore.Core.Application.Interfaces.Handler;
using MediatR;

namespace AmazingMicroStore.Core.InfraStructure.Handler
{
    public class Handler : IHandler
    {
        #region Fields

        private readonly IMediator _mediator;

        #endregion Fields

        #region Constructors

        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion Constructors

        #region Methods

        public Task RaiseEvent<T>(T @event) where T : Event
        {
            var result = _mediator.Publish(@event);
            return result;
        }

        #endregion Methods
    }
}