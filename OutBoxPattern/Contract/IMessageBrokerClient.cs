public interface IMessageBrokerClient
{
    Task ConnectAsync();
    Task DisconnectAsync();
    Task<bool> IsConnectedAsync();
    Task PublishAsync(string eventId, string eventType, string payload);
}
