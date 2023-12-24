using System.Text;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using DispatcherService.Models;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading.Channels;
using RabbitMQ.Client.Events;
//using (var connection = factory.CreateConnection())
//using (var channel = connection.CreateModel())

namespace DispatcherService.Services
{
    public class MessageBrokerClientService : IMessageBrokerClient
    {
        private readonly RabbitMQClientManager _rabbitMQClientManager;
        private ILogger<MessageBrokerClientService> _logger;

        public MessageBrokerClientService(RabbitMQClientManager rabbitMQClientManager, ILogger<MessageBrokerClientService> logger)
        {
            _rabbitMQClientManager = rabbitMQClientManager;
            _logger = logger;
        }

        public Task ConnectAsync()
        {
            _logger.LogInformation("connection is establised");
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            _rabbitMQClientManager.Close();
            _logger.LogInformation("connection is closed");
            return Task.CompletedTask;
        }

        public async Task PublishAsync<T>(string eventType, string subEventType,  T message)
        {
            _rabbitMQClientManager.Channel.ExchangeDeclare(exchange: "EventExchange", type: ExchangeType.Direct);
            _rabbitMQClientManager.Channel.QueueDeclare(queue: "Event", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _rabbitMQClientManager.Channel.QueueBind(queue: "Service", exchange: "EventExchange", routingKey: eventType);
            var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var properties = _rabbitMQClientManager.Channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.DeliveryMode = 2;
            properties.Expiration = "36000000";
            properties.CorrelationId=  "a";
           // properties.Headers= new Dictionary(1, "a");
            _rabbitMQClientManager.Channel.BasicPublish(exchange: "EventExchange", routingKey: eventType, basicProperties: properties, body: messageBodyBytes);


            //var consumer = new EventingBasicConsumer(_channel);

            //consumer.Received += (model, ea) =>
            //{
            //    var message = BinarySerializer.Deserilize<NotificationMessage>(ea.Body.ToArray());
            //};

            //_channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        }
    }
    public class RabbitMQClientManager
    {
        private readonly RabbitMQConfigModel _config;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQClientManager(IOptions<RabbitMQConfigModel> config)
        {
            _config = config.Value;

            var factory = new ConnectionFactory
            {
                HostName = _config.Host,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public IModel Channel => _channel;

        public void Close()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }


}

