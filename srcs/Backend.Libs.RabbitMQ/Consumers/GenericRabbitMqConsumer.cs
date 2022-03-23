using System.Reflection;
using System.Threading.Channels;
using Backend.Libs.RabbitMQ.Attributes;
using Backend.Libs.RabbitMQ.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Backend.Libs.RabbitMQ.Consumers;

public class GenericRabbitMqConsumer<T> : IRabbitMqConsumer<T>,
    IHostedService,
    IDisposable
    where T : IRabbitMqMessage<T>
{
    private readonly ILogger _logger;
    private readonly IAsyncRabbitMqConsumerEventHandler<T> _asyncConsumerEventHandler;
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public GenericRabbitMqConsumer(ILoggerFactory logger, ConnectionFactory connectionFactory, IAsyncRabbitMqConsumerEventHandler<T> asyncConsumerEventHandler)
    {
        _logger = logger.CreateLogger(typeof(GenericRabbitMqConsumer<T>));
        _asyncConsumerEventHandler = asyncConsumerEventHandler;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task StartAsync(CancellationToken cancellationToken)
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

    private async Task ConsumerOnReceived(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            await _asyncConsumerEventHandler.HandleAsync(sender, @event);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GenericRabbitMqConsumer<T>));
        }
        finally
        {
            _channel.BasicAck(@event.DeliveryTag, false);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public void Dispose()
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