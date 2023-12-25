using DispacherApplication.Broker;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace DispatcherService.Services
{
    public class MessageBrokerClientService : IMessageBrokerClient
    {
        private readonly RabbitMQClientManager _rabbitClient;
        private const string QueueName = "Events";

        public MessageBrokerClientService(RabbitMQClientManager rabbitMQClientManager)
        {
            _rabbitClient = rabbitMQClientManager;
        }

        public void Connect()
        {
            _rabbitClient.Open();
            Log.Logger.Information("connection is open");
            _rabbitClient.Channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            Log.Logger.Information("queue declared");
        }

        public void Disconnect()
        {
            _rabbitClient.Close();
            Log.Logger.Information("connection is closed");
        }

        public bool IsConnected()
        {
            return true;
        }


        public void Publish(string eventId, string eventType, string payload)
        {
            _rabbitClient.Open();
            var body = Encoding.UTF8.GetBytes(payload);
            var properties = _rabbitClient.Channel.CreateBasicProperties();

            properties.ContentType = "application/json";
            properties.DeliveryMode = 2;
            properties.CorrelationId = eventId;
            properties.Headers = new Dictionary<string, object>()
            {
                { "eventType",eventType }
            };

            _rabbitClient.Channel.BasicPublish(exchange: string.Empty, QueueName, properties, body);
        }
    }
}