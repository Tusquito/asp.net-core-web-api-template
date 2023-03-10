using System.Reflection;
using System.Text;
using System.Text.Json;
using Backend.Libs.Infrastructure.Attributes;
using Backend.Libs.Infrastructure.Messages.Abstractions;
using Backend.Libs.Infrastructure.Producers.Abstractions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Backend.Libs.Infrastructure.Producers;

public class RabbitMqProducer : IMessageProducer
{
    private readonly ConnectionFactory _connection;
    private readonly ILogger<RabbitMqProducer> _logger;

    public RabbitMqProducer(ConnectionFactory connection, ILogger<RabbitMqProducer> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public Task ProduceAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage
    {
        try
        {
            using IConnection? connection = _connection.CreateConnection();

            if (connection == null)
            {
                _logger.LogError("[{Scope}] An error occured while opening a connection",
                    nameof(RabbitMqProducer));
                return Task.CompletedTask;
            }
            
            string? queueName = typeof(T).GetCustomAttribute<QueueNameAttribute>()?.Name;

            if (string.IsNullOrEmpty(queueName))
            {
                _logger.LogError("[{Scope}] No queue name found for {Message}",
                    nameof(RabbitMqProducer),
                    nameof(T));
                return Task.CompletedTask;
            }

            using IModel? channel = connection.CreateModel();

            if (channel == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating a channel",
                    nameof(RabbitMqProducer));
                return Task.CompletedTask;
            }
            
            QueueDeclareOk? queueDeclare = channel.QueueDeclare(queueName, false, false, false, null);

            if (queueDeclare == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating the queue {Name}",
                    nameof(RabbitMqProducer),
                    queueName);
                return Task.CompletedTask;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            IBasicProperties? props = channel.CreateBasicProperties();
            channel.BasicPublish(string.Empty, queueName, props, bytes);
            _logger.LogInformation("[{Scope}] New message sent into queue {QueueName}", nameof(RabbitMqProducer), queueName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(RabbitMqProducer));
        }
        
        return Task.CompletedTask;
    }

    public Task ProduceAsync<T>(List<T> messages, CancellationToken cancellationToken = default) where T : IMessage
    {
        try
        {
            using IConnection? connection = _connection.CreateConnection();

            if (connection == null)
            {
                _logger.LogError("[{Scope}] An error occured while opening a connection",
                    nameof(RabbitMqProducer));
                return Task.CompletedTask;
            }
            
            string? queueName = typeof(T).GetCustomAttribute<QueueNameAttribute>()?.Name;

            if (string.IsNullOrEmpty(queueName))
            {
                _logger.LogError("[{Scope}] No queue name found for {Message}",
                    nameof(RabbitMqProducer),
                    nameof(T));
                return Task.CompletedTask;
            }

            using IModel? channel = connection.CreateModel();

            if (channel == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating a channel",
                    nameof(RabbitMqProducer));
                return Task.CompletedTask;
            }
            
            QueueDeclareOk? queueDeclare = channel.QueueDeclare(queueName, false, false, false, null);

            if (queueDeclare == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating the queue {Name}",
                    nameof(RabbitMqProducer),
                    queueName);
                return Task.CompletedTask;
            }

            IBasicPublishBatch? batch = channel.CreateBasicPublishBatch();

            if (batch == null)
            {
                _logger.LogError("[{Scope}] An error occured while creating batch for messages of type {Type}",
                    nameof(RabbitMqProducer),
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
                nameof(RabbitMqProducer),
                messages.Count,
                queueName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(RabbitMqProducer));
        }
        
        return Task.CompletedTask;
    }
}