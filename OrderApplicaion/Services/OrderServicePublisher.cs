using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using RabbitMQ;
using OrderApplicaion.Contract;
using Microsoft.Extensions.Options;
using OrderApplicaion.Models;


namespace OrderApplicaion.Services
{
    public class OrderServicePublisher : IMessageProducer
    {
        private readonly RabbitMQConfig _config;

        public OrderServicePublisher(IOptions<RabbitMQConfig> config)
        {
            _config = config.Value;
        }

        public void Publish<T>(string routingKey, T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config.Host,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password
            };

            // ... existing publishing logic
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateChannel())
            {
                // Declare the exchange if needed
                // channel.ExchangeDeclare(exchange: "your_exchange", type: ExchangeType.Direct);

                var body = Encoding.UTF8.GetBytes(CreateRabbitMqMessageBody(message));

                //channel.BasicPublish(exchange: "", routingKey: routingKey, basicProperties: null, body: body);
                channel.BasicPublish(exchange: "", routingKey: routingKey, body: body);
            }
        }

        private string CreateRabbitMqMessageBody<T>(T message)
        {
            // Serialize your message object to a string
            // For example, using JSON:
            return Newtonsoft.Json.JsonConvert.SerializeObject(message);
        }
    }
}
