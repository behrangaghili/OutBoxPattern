namespace OutBoxPattern.Contract
{
    public interface IMessageProducer
    {
        void Publish<T>(string routingKey, T message);
        
    }

}
