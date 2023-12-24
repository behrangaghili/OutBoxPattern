public interface IMessageBrokerClient
{
    Task ConnectAsync();
    Task DisconnectAsync();
    Task PublishAsync<T>(string eventType, string subEventType, T message);
}
