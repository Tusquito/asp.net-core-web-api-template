using Backend.Libs.Mediator.Messaging.Abstractions;
using Backend.Libs.Messaging.Abstractions;
using Backend.Libs.Messaging.Consumers;
using Backend.Libs.Messaging.Producers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

namespace Backend.Plugins.Messaging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddRabbitMqConsumer<TMessage>(this IServiceCollection services)
        where TMessage : IMessage<TMessage>, IRequest
    {
        services.TryAddTransient<IMessageConsumer<TMessage>, GenericRabbitMqConsumer<TMessage>>();
        services.AddHostedService<GenericRabbitMqConsumer<TMessage>>();
        return services;
    }
    
    public static IServiceCollection TryAddRabbitMqProducer<TMessage>(this IServiceCollection services)
        where TMessage : IMessage<TMessage>
    {
        services.TryAddTransient<IMessageProducer<TMessage>, GenericRabbitMqProducer<TMessage>>();
        return services;
    }

    public static IServiceCollection TryAddRabbitMqClientFactory(this IServiceCollection services)
    {
        services.TryAddSingleton(_ => new ConnectionFactory
        {
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "bitnami",
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "user",
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? "localhost",
            Port = short.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
            DispatchConsumersAsync = true
        });

        return services;
    }
}