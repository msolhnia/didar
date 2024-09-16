using EventBus.Interface;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Packaging.infrastructure.EventBus
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQEventBus(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish(string eventName, object eventData)
        {
            _channel.ExchangeDeclare(exchange: "module_exchange", type: "direct");
            var message = JsonSerializer.Serialize(eventData);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "module_exchange", routingKey: eventName, body: body);
        }
    }

}
