namespace NotificationMicroservice.EventProcessors
{
    public interface IEventProcessor
    {
        string EventType { get; }

        Task Process(string messageId, string subEventType, byte[] payload);
    }
}
