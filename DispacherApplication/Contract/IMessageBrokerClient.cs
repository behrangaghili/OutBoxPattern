public interface IMessageBrokerClient
{
    void Connect();
    void Disconnect();
    bool IsConnected();
    void Publish(string eventId, string eventType, string payload);
}
