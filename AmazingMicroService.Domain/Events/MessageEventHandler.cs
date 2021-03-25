using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;

namespace AmazingMicroService.Domain.Events
{
    public class MessageEventHandler :
        IRequestHandler<MessageEvent>

    {
        #region Constructors

        public MessageEventHandler()
        {
        }

        #endregion Constructors

        #region Methods

        public async Task<Unit> Handle(MessageEvent request, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(request));

            return Unit.Value;
        }

        #endregion Methods
    }
}