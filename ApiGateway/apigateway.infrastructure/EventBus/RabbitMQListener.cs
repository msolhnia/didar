using apigateway.infrastructure.Cache;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace apigateway.infrastructure.EventBus
{
    public class RabbitMQListener : IHostedService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RedisCacheService _cacheService;

        public RabbitMQListener(IConnectionFactory connectionFactory, RedisCacheService cacheService)
        {
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _cacheService = cacheService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _channel.ExchangeDeclare(exchange: "module_exchange", type: "direct");
            _channel.QueueDeclare(queue: "module_update_queue", durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Parse the message to get module data
                var moduleUpdate = JsonSerializer.Deserialize<ModuleUpdate>(message);

                // Update the Redis cache
                await _cacheService.SetModuleCacheAsync(moduleUpdate.ModuleId.ToString(), message);
            };

            _channel.BasicConsume(queue: "module_update_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            return Task.CompletedTask;
        }
    }

}
