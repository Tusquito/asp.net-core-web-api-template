using Backend.Libs.RabbitMQ;
using Backend.Libs.RabbitMQ.Consumers;
using Backend.Libs.RabbitMQ.Events;
using Backend.Libs.RabbitMQ.Producers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

namespace Backend.Plugins.RabbitMQ.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqConsumer<TMessage, TEvent>(this IServiceCollection services)
        where TMessage : IRabbitMqMessage<TMessage>
        where TEvent : class, IAsyncRabbitMqConsumerEventHandler<TMessage>
    {
        services.TryAddSingleton<IAsyncRabbitMqConsumerEventHandler<TMessage>, TEvent>();
        services.TryAddTransient<IRabbitMqConsumer<TMessage>, GenericRabbitMqConsumer<TMessage>>();
        services.AddHostedService<GenericRabbitMqConsumer<TMessage>>();
        return services;
    }
    
    public static IServiceCollection AddRabbitMqProducer<TMessage>(this IServiceCollection services)
        where TMessage : IRabbitMqMessage<TMessage>
    {
        services.TryAddTransient<IRabbitMqProducer<TMessage>, GenericRabbitMqProducer<TMessage>>();
        return services;
    }

    public static IServiceCollection AddRabbitMqClientFactoryFromEnv(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceUri = configuration.GetServiceUri("stnoks-rabbitmq");
        
        services.TryAddSingleton(_ => new ConnectionFactory
        {
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "bitnami",
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "user",
            HostName = serviceUri == null ? "localhost" : serviceUri.Host,
            Port = serviceUri == null ? 5672 : serviceUri.Port,
            DispatchConsumersAsync = true
        });

        return services;
    }
}