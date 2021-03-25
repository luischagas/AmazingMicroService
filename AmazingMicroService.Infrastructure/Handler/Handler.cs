using AmazingMicroService.DomainService.Interfaces.Handler;
using MediatR;
using System.Threading.Tasks;

namespace AmazingMicroService.Infrastructure.Handler
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

        public Task Send<T>(T @event)
        {
            var result = _mediator.Send(@event);
            return result;
        }

        #endregion Methods
    }
}