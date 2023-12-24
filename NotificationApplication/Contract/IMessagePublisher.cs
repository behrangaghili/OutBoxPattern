namespace DispatcherService.Contract
{
    public interface IMessagePublisher
    {
        void Publish<T>(string routingKey, T message);
        
    }

}
