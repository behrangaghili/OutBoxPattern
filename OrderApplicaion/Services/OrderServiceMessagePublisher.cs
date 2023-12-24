using System.Text;
using RabbitMQ.Client;
using DispatcherService.Contract;
using Microsoft.Extensions.Options;
using DispatcherService.Models;
using System.Threading.Channels;
using Newtonsoft.Json;

namespace DispatcherService.Services
{
    public class OrderServiceMessagePublisher : IMessagePublisher
    {
        private readonly RabbitMQConfigModel _config;
        private readonly ILogger<OrderServiceMessagePublisher> _logger;  

        public OrderServiceMessagePublisher(IOptions<RabbitMQConfigModel> config, ILogger<OrderServiceMessagePublisher> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public void Publish<T>(string routingKey, T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.Host,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password
            };

 

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "EventExchange", type: ExchangeType.Direct);
                channel.QueueDeclare(queue: "Service", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: "Service", exchange: "EventExchange", routingKey: routingKey);

                var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.ContentType = "application/json";
                properties.DeliveryMode = 2;
                properties.Expiration = "36000000";

                channel.BasicPublish(exchange: "EventExchange", routingKey: routingKey, basicProperties: properties, body: messageBodyBytes);
            }
        }

    }
}
