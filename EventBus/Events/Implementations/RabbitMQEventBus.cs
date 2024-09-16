using EventBus.Events.Handlers;
using EventBus.Interface;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace EventBus.Events.Implementations
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQEventBus(IConnectionFactory connectionFactory, IServiceProvider serviceProvider)
        {
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _serviceProvider = serviceProvider;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IntegrationEvent
        {
            var eventName = typeof(TEvent).Name;
            _channel.ExchangeDeclare(exchange: "event_bus", type: "direct");

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "event_bus", routingKey: eventName, body: body);
        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = typeof(TEvent).Name;
            _channel.ExchangeDeclare(exchange: "event_bus", type: "direct");
            _channel.QueueDeclare(queue: eventName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: eventName, exchange: "event_bus", routingKey: eventName);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var @event = JsonSerializer.Deserialize<TEvent>(message);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetRequiredService<TEventHandler>();
                    await handler.Handle(@event);
                }
            };

            _channel.BasicConsume(queue: eventName, autoAck: true, consumer: consumer);
        }
    }
}
