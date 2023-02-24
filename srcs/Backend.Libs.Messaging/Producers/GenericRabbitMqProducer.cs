using System.Reflection;
using System.Text;
using System.Text.Json;
using Backend.Libs.Mediator.Messaging.Abstractions;
using Backend.Libs.Messaging.Abstractions;
using Backend.Libs.Messaging.Attributes;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Backend.Libs.Messaging.Producers;

public class GenericRabbitMqProducer<T> : IMessageProducer<T>
    where T : IMessage<T>
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
            using IConnection? connection = _connection.CreateConnection();

            if (connection == null)
            {
                _logger.LogError("[{Scope}] An error occured while opening a connection",
                    nameof(GenericRabbitMqProducer<T>));
                return Task.CompletedTask;
            }
            
            string? queueName = typeof(T).GetCustomAttribute<QueueNameAttribute>()?.Name;

            if (string.IsNullOrEmpty(queueName))
            {
                _logger.LogError("[{Scope}] No queue name found for {Message}",
                    nameof(GenericRabbitMqProducer<T>),
                    nameof(T));
                return Task.CompletedTask;
            }

            using IModel? channel = connection.CreateModel();

            if (channel == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating a channel",
                    nameof(GenericRabbitMqProducer<T>));
                return Task.CompletedTask;
            }
            
            QueueDeclareOk? queueDeclare = channel.QueueDeclare(queueName, false, false, false, null);

            if (queueDeclare == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating the queue {Name}",
                    nameof(GenericRabbitMqProducer<T>),
                    queueName);
                return Task.CompletedTask;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            IBasicProperties? props = channel.CreateBasicProperties();
            channel.BasicPublish(string.Empty, queueName, props, bytes);
            _logger.LogInformation("[{Scope}] New message sent into queue {QueueName}", nameof(GenericRabbitMqProducer<T>), queueName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GenericRabbitMqProducer<T>));
        }
        
        return Task.CompletedTask;
    }

    public Task ProduceAsync(List<T> messages, CancellationToken cancellationToken = default)
    {
        try
        {
            using IConnection? connection = _connection.CreateConnection();

            if (connection == null)
            {
                _logger.LogError("[{Scope}] An error occured while opening a connection",
                    nameof(GenericRabbitMqProducer<T>));
                return Task.CompletedTask;
            }
            
            string? queueName = typeof(T).GetCustomAttribute<QueueNameAttribute>()?.Name;

            if (string.IsNullOrEmpty(queueName))
            {
                _logger.LogError("[{Scope}] No queue name found for {Message}",
                    nameof(GenericRabbitMqProducer<T>),
                    nameof(T));
                return Task.CompletedTask;
            }

            using IModel? channel = connection.CreateModel();

            if (channel == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating a channel",
                    nameof(GenericRabbitMqProducer<T>));
                return Task.CompletedTask;
            }
            
            QueueDeclareOk? queueDeclare = channel.QueueDeclare(queueName, false, false, false, null);

            if (queueDeclare == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating the queue {Name}",
                    nameof(GenericRabbitMqProducer<T>),
                    queueName);
                return Task.CompletedTask;
            }

            IBasicPublishBatch? batch = channel.CreateBasicPublishBatch();

            if (batch == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating batch for messages of type {Type}",
                    nameof(GenericRabbitMqProducer<T>),
                    typeof(T));
                return Task.CompletedTask;
            }
            
            IBasicProperties? props = channel.CreateBasicProperties();

            foreach (T message in messages)
            {
                batch.Add(string.Empty, queueName, false, props,
                    new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message))));
            }

            batch.Publish();
            _logger.LogInformation("[{Scope}] {Count} new messages has been sent into queue {QueueName}",
                nameof(GenericRabbitMqProducer<T>),
                messages.Count,
                queueName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GenericRabbitMqProducer<T>));
        }
        
        return Task.CompletedTask;
    }
}