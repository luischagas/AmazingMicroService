using System;

namespace AmazingMicroService.Domain.Models
{
    public class Subscription
    {
        #region Constructors

        public Subscription(Type handlerType)
        {
            HandlerType = handlerType;
        }

        #endregion Constructors

        #region Properties

        public Type HandlerType { get; }

        #endregion Properties
    }
}