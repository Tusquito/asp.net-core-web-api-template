using System.Reflection;
using System.Text;
using System.Text.Json;
using Backend.Libs.Mediator.Messaging.Abstractions;
using Backend.Libs.Messaging.Attributes;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Backend.Libs.Messaging.Producers;

public class GenericRabbitMqProducer<T> : IRabbitMqProducer<T>
    where T : IRabbitMqMessage<T>
{
    private readonly ConnectionFactory _connection;
    private readonly ILogger<GenericRabbitMqProducer<T>> _logger;

    public GenericRabbitMqProducer(ConnectionFactory connection, ILogger<GenericRabbitMqProducer<T>> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public Task ProduceAsync(T message, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = _connection.CreateConnection();
            var queueName = typeof(T).GetCustomAttribute<QueueNameAttribute>()?.Name;

            if (string.IsNullOrEmpty(queueName))
            {
                _logger.LogError("[{Scope}] No queue name found for {Message}", nameof(GenericRabbitMqProducer<T>),
                    nameof(T));
                return Task.CompletedTask;
            }

            using var channel = connection.CreateModel();
            channel.QueueDeclare(queueName, false, false, false, null);

            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = channel.CreateBasicProperties();
            channel.BasicPublish(string.Empty, queueName, props, bytes);
            _logger.LogInformation("[{Scope}] New message sent into queue {QueueName}", nameof(GenericRabbitMqProducer<T>), queueName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GenericRabbitMqProducer<T>));
        }
        
        return Task.CompletedTask;
    }
}