using DispatcherService.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DispacherApplication.Broker
{
    public class RabbitMQClientManager
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQClientManager(IConfiguration config)
        {
            var host = config["RabbiqMQ:Host"];

            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",//config["RabbiqMQ:Host"],
                //Port = 5671,//Convert.ToInt32(config["RabbiqMQ:Port"]),
                UserName = "guest",//*config["RabbiqMQ:Username"],*/
                Password = "guest",//config["RabbiqMQ:Password"]
            };
        }

        public void Open()
        {
            _connection = _connectionFactory.CreateConnection();
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
