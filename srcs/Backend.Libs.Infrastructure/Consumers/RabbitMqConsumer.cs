﻿using System.Reflection;
using System.Text;
using System.Text.Json;
using Backend.Libs.Infrastructure.Attributes;
using Backend.Libs.Infrastructure.Consumers.Abstractions;
using Backend.Libs.Infrastructure.Messages.Abstractions;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Backend.Libs.Infrastructure.Consumers;

public class RabbitMqConsumer<T> : IMessageConsumer<T>,
    IHostedService,
    IDisposable
    where T : IMessage
{
    private readonly ILogger _logger;
    private IModel _channel;
    private IConnection _connection;
    private readonly ISender _sender;
    
    public RabbitMqConsumer(ILoggerFactory logger, ConnectionFactory connectionFactory, ISender sender)
    {
        _sender = sender;
        _logger = logger.CreateLogger(typeof(RabbitMqConsumer<T>));
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
                _logger.LogError("[{Scope}] No queue name found for {Message}", nameof(RabbitMqConsumer<T>),
                    nameof(T));
                return Task.CompletedTask;
            }

            _channel.QueueDeclare(queueName, false, false, false, null);

            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += ConsumerOnReceived;

            _channel.BasicConsume(queueName, false, consumer);
            _logger.LogInformation("[{Scope}] New consumer registered for {QueueName}",
                nameof(RabbitMqConsumer<T>), queueName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(RabbitMqConsumer<T>));
        }
        return Task.CompletedTask;
    }

    private async Task ConsumerOnReceived(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            await _sender.Send(JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(@event.Body.Span))!);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(RabbitMqConsumer<T>));
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
            _channel = null!;
        }

        if (_connection is { IsOpen: true })
        {
            _connection.Close();
            _connection.Dispose();
            _connection = null!;
        }
    }
}