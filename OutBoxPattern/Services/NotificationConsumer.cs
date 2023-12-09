using OutBoxPattern.Contract;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OutBoxPattern.Services
{
    public class NotificationConsumer<OutboxEvent>  
    {
        public void Consume<OutboxEvent1>(string routingKey, OutboxEvent message)
        {
            throw new NotImplementedException();
        }
    }
}

