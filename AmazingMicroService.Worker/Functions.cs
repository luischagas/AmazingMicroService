using AmazingMicroService.Application.Events.MessageEvents.Input;
using AmazingMicroStore.Core.Application.Interfaces.Handler;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingMicroService.Worker
{
    public class Functions : BackgroundService
    {
        #region Fields

        private readonly IHandler _handler;

        #endregion Fields

        #region Constructors

        public Functions(IHandler handler)
        {
            _handler = handler;
        }

        #endregion Constructors

        #region Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested is false)
            {
                await _handler.RaiseEvent(new SendMessageEvent
                {
                    MicroServiceId = Program.ApplicationName,
                    Message = "Hello World"
                });

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        #endregion Methods
    }
}