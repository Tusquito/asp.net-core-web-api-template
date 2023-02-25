using Backend.Libs.Mediator.Messaging.Abstractions;
using Backend.Libs.Messaging.Abstractions;
using Backend.Libs.Messaging.Consumers;
using Backend.Libs.Messaging.Options;
using Backend.Libs.Messaging.Producers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Backend.Libs.Messaging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddRabbitMqConsumer<TMessage>(this IServiceCollection services)
        where TMessage : IMessage, IRequest
    {
        services.TryAddTransient<IMessageConsumer<TMessage>, GenericRabbitMqConsumer<TMessage>>();
        services.AddHostedService<GenericRabbitMqConsumer<TMessage>>();
        return services;
    }
    
    public static IServiceCollection TryAddRabbitMqProducer<TMessage>(this IServiceCollection services)
        where TMessage : IMessage
    {
        services.TryAddTransient<IMessageProducer<TMessage>, GenericRabbitMqProducer<TMessage>>();
        return services;
    }

    public static IServiceCollection TryAddRabbitMqClientFactory(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.Name));
        services.AddSingleton<RabbitMqOptions>(x => x.GetRequiredService<IOptions<RabbitMqOptions>>().Value);
        
        services.TryAddSingleton(provider =>
        {
            RabbitMqOptions options = provider.GetRequiredService<RabbitMqOptions>();
            
            return new ConnectionFactory
            {
                Password = options.Password,
                UserName = options.Username,
                HostName = options.Hostname,
                Port = options.Port,
                DispatchConsumersAsync = true
            };
        });

        return services;
    }
}