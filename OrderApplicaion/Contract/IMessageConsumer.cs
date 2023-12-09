namespace OrderApplicaion.Contract
{
    public interface IMessageConsumer
    {
        void Consume<T>(string routingKey, T message);
    }
}
