using NotificationMicroservice.Data;
using NotificationMicroservice.EventProcessors;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace NotificationMicroservice.Application
{
    public class EventMessageQueue : IEventMessageQueue, IDisposable
    {
        private const string QueueName = "notification_queue";
        private readonly AppDataContext _db;
        private readonly EventProcessorFactory eventProcessorFactory;
        private IConnection? _conn;
        private IModel? _channel;

        public EventMessageQueue(AppDataContext db, EventProcessorFactory eventProcessorFactory)
        {
            _db = db;
            this.eventProcessorFactory = eventProcessorFactory;
        }

        public void Start()
        {
            CreateConnection();
            ConsumeMessages();
        }

        private void ConsumeMessages()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var message = BinarySerializer.Deserilize<NotificationMessage>(ea.Body.ToArray());
                var eventType = ea.BasicProperties.Headers["eventType"].ToString();
                var subEventType = ea.BasicProperties.Headers["subEventType"].ToString();
                var messageId = ea.BasicProperties.CorrelationId;
                var payload = ea.Body.ToArray();

                var processor = eventProcessorFactory.Create(eventType);
                processor.Process(messageId, subEventType, payload);
            };

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _conn = factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Dispose()
        {
            _conn.Dispose();
            _channel.Dispose();
        }
    }
}