using System;
using RabbitMQ.Client;

namespace Core
{
    public class RabbitMqService : IDisposable
    {
        readonly string _host;
        readonly IConnection _connection;

        public RabbitMqService(string host)
        {
            _host = host;

            var factory = new ConnectionFactory
            {
                Protocol = Protocols.FromEnvironment(),
                HostName = _host
            };

            _connection = factory.CreateConnection();
        }

        public IModel CreateModel(string exchange, string queue, string key)
        {
            var model = _connection.CreateModel();

            model.ExchangeDeclare(exchange, ExchangeType.Direct);
            model.QueueDeclare(queue, false, false, false, null);
            model.QueueBind(queue, exchange, key);

            return model;
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}