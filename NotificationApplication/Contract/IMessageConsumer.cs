namespace DispatcherService.Contract
{
    public interface IMessageConsumer
    {
       // void Consume<T>(Guid eventId, string eventType , string subEventType, T message);
        void Consume<T>(  string eventType , string message);
    }
}
