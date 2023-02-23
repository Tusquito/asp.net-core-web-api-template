using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Backend.Libs.RabbitMQ.Attributes;
using Backend.Libs.RabbitMQ.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Backend.Libs.RabbitMQ.Consumers;

public class GenericRabbitMqConsumer<T> : BackgroundService, IRabbitMqConsumer<T> where T : IRabbitMqMessage<T>
{
    private readonly ILogger _logger;
    private readonly IAsyncRabbitMqConsumerMessageHandler<T> _asyncConsumerMessageHandler;
    private readonly ConcurrentQueue<(T, BasicDeliverEventArgs)> _queue;
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public GenericRabbitMqConsumer(ILoggerFactory logger, ConnectionFactory connectionFactory, IAsyncRabbitMqConsumerMessageHandler<T> asyncConsumerMessageHandler, ConcurrentQueue<(T, BasicDeliverEventArgs)> queue)
    {
        _logger = logger.CreateLogger(typeof(GenericRabbitMqConsumer<T>));
        _asyncConsumerMessageHandler = asyncConsumerMessageHandler;
        _queue = queue;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        while (await timer.WaitForNextTickAsync(cancellationToken) && !cancellationToken.IsCancellationRequested)
        {
            int count = 0;
            while (_queue.TryDequeue(out var queuedMsg))
            {
                try
                {
                    await _asyncConsumerMessageHandler.HandleAsync(queuedMsg.Item1, cancellationToken);
                    _logger.LogDebug("[{Scope}] New message of type {Type} consumed successfully",
                        nameof(GenericRabbitMqConsumer<T>), typeof(T));
                    count++;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "[{Scope}]", nameof(GenericRabbitMqConsumer<T>));
                }
                finally
                {
                    _channel.BasicAck(queuedMsg.Item2.DeliveryTag, false);
                }
            }
            _logger.LogInformation("[{Scope}] {Count} messages consumed successfully",
                nameof(GenericRabbitMqConsumer<T>), count);
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }
        
        try
        {
            string? queueName = typeof(T).GetCustomAttribute<QueueNameAttribute>()?.Name;

            if (string.IsNullOrEmpty(queueName))
            {
                _logger.LogError("[{Scope}] No queue name found for {Message}", nameof(GenericRabbitMqConsumer<T>),
                    nameof(T));
                return Task.CompletedTask;
            }

            _channel.QueueDeclare(queueName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += ConsumerOnReceived;

            _channel.BasicConsume(queueName, false, consumer);
            _logger.LogInformation("[{Scope}] New consumer registered for {QueueName}",
                nameof(GenericRabbitMqConsumer<T>), queueName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GenericRabbitMqConsumer<T>));
        }
        return Task.CompletedTask;
    }

    private Task ConsumerOnReceived(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            _queue.Enqueue((JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(@event.Body.ToArray()))!, @event));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GenericRabbitMqConsumer<T>));
        }

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public override void Dispose()
    {
        if (_channel is { IsOpen: true })
        {
            _channel.Close();
            _channel.Dispose();
        }

        if (_connection is { IsOpen: true })
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}