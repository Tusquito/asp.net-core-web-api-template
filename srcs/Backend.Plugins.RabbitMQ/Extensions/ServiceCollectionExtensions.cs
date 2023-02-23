using Backend.Libs.RabbitMQ;
using Backend.Libs.RabbitMQ.Consumers;
using Backend.Libs.RabbitMQ.Handlers;
using Backend.Libs.RabbitMQ.Publishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

namespace Backend.Plugins.RabbitMQ.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqConsumer<TMessage, TEvent>(this IServiceCollection services)
        where TMessage : IRabbitMqMessage<TMessage>
        where TEvent : class, IAsyncRabbitMqConsumerMessageHandler<TMessage>
    {
        services.TryAddSingleton<IAsyncRabbitMqConsumerMessageHandler<TMessage>, TEvent>();
        services.TryAddTransient<IRabbitMqConsumer<TMessage>, GenericRabbitMqConsumer<TMessage>>();
        services.AddHostedService<GenericRabbitMqConsumer<TMessage>>();
        return services;
    }
    
    public static IServiceCollection AddRabbitMqPublisher<TMessage>(this IServiceCollection services)
        where TMessage : IRabbitMqMessage<TMessage>
    {
        services.TryAddTransient<IRabbitMqPublisher<TMessage>, GenericRabbitMqPublisher<TMessage>>();
        return services;
    }

    public static IServiceCollection AddRabbitMqClientFactoryFromEnv(this IServiceCollection services)
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