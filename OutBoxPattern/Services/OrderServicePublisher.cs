using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ;
using OutBoxPattern.Contract;
// Add other necessary using statements here

namespace OutBoxPattern.Services
{
    public class OrderServicePublisher : IMessageProducer
    {
        public void Publish<T>(string routingKey, T message)
        {
            throw new NotImplementedException();
        }
    }
}
