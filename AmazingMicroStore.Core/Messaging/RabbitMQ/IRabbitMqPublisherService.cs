namespace AmazingMicroStore.Core.Application.Messaging.RabbitMQ
{
    public interface IRabbitMqPublisherService
    {
        #region Methods

        bool EnqueueMessage(string message);

        #endregion Methods
    }
}