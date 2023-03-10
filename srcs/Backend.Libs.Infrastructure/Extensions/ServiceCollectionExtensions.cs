using Backend.Libs.Domain.Extensions;
using Backend.Libs.Infrastructure.Consumers;
using Backend.Libs.Infrastructure.Consumers.Abstractions;
using Backend.Libs.Infrastructure.Enums;
using Backend.Libs.Infrastructure.Messages.Abstractions;
using Backend.Libs.Infrastructure.Options;
using Backend.Libs.Infrastructure.Producers;
using Backend.Libs.Infrastructure.Producers.Abstractions;
using Backend.Libs.Infrastructure.Repositories;
using Backend.Libs.Infrastructure.Repositories.Abstractions;
using Backend.Libs.Infrastructure.Serializers;
using Backend.Libs.Infrastructure.Services.Account;
using Backend.Libs.Infrastructure.Services.Account.Abstractions;
using Foundatio.Caching;
using Grpc.Net.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProtoBuf.Grpc.Client;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace Backend.Libs.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLibs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly))
            .TryAddRedisRepositories(configuration);

        return services;
    }
    
    #region RabbitMQ
    
    public static IServiceCollection TryAddRabbitMqConsumer<TMessage>(this IServiceCollection services, IConfiguration configuration)
        where TMessage : IMessage, IRequest
    {
        services.TryAddRabbitMqClientFactory(configuration);
        services.TryAddTransient<IMessageConsumer<TMessage>, RabbitMqConsumer<TMessage>>();
        services.AddHostedService<RabbitMqConsumer<TMessage>>();
        return services;
    }

    public static IServiceCollection TryAddRabbitMqProducer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.TryAddRabbitMqClientFactory(configuration);
        services.TryAddTransient<IMessageProducer, RabbitMqProducer>();
        return services;
    }

    public static IServiceCollection TryAddRabbitMqClientFactory(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryConfigure<RabbitMqOptions>(configuration);
        services.TryAddSingleton(provider =>
        {
            RabbitMqOptions rabbitMqOptions = provider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            
            return new ConnectionFactory
            {
                Password = rabbitMqOptions.Password,
                UserName = rabbitMqOptions.Username,
                HostName = rabbitMqOptions.Hostname,
                Port = rabbitMqOptions.Port,
                DispatchConsumersAsync = true
            };
        });

        return services;
    }
    
    #endregion

    #region Redis

    private static IServiceCollection TryAddRedisRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryConfigure<RedisOptions>(configuration);
        services.TryAddSingleton<IConnectionMultiplexer>(s =>
        {
            RedisOptions conf = s.GetRequiredService<IOptions<RedisOptions>>().Value;
                
            return ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                Password = conf.Password,
                EndPoints = { conf.ToString() }
            });
        });
        
        services.TryAddSingleton(s => new RedisCacheClient(new RedisCacheClientOptions
        {
            ConnectionMultiplexer = s.GetRequiredService<IConnectionMultiplexer>(),
            Serializer = new ProtoSerializer()
        }));
        
        services.TryAddSingleton<ICacheClient>(s => s.GetRequiredService<RedisCacheClient>());
        
        services.TryAddSingleton(typeof(IKeyValueRepository<,>), typeof(GenericRedisKeyValueRepository<,>));

        return services;
    }

    #endregion

    #region Grpc
    
    private static void AddGrpcClientService<T>(this IServiceCollection services, ServicePort servicePort) where T : class
    {
        services.AddScoped<T>(s =>
        {
            string ip = Environment.GetEnvironmentVariable(servicePort.ToString()) ?? "localhost";
            int port = Convert.ToInt32(servicePort);
            //CallCredentials credentials = CallCredentials.FromInterceptor((_, _) => Task.CompletedTask);
            
            return GrpcChannel.ForAddress($"http://{ip}:{port}", new GrpcChannelOptions
            {
                MaxReceiveMessageSize = null,
                MaxSendMessageSize = null,
                MaxRetryAttempts = 3,
                LoggerFactory = s.GetRequiredService<ILoggerFactory>(),
                //Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
            }).CreateGrpcService<T>();
        });
    }
    
    public static IServiceCollection AddGrpcDatabaseServices(this IServiceCollection services)
    {
        services.AddTransient<IAccountService, AccountService>();
        services.AddGrpcClientService<IGrpcAccountService>(ServicePort.DATABASE_SERVER_PORT);
        return services;
    }

    #endregion
}