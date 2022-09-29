using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Enums;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;

namespace Backend.Libs.gRPC.Extensions;

public static class ServiceCollectionExtensions
{
    private static void AddGrpcClientService<T>(this IServiceCollection services, GrpcServiceType serviceType) where T : class
    {
        string ip = Environment.GetEnvironmentVariable(serviceType.ToString()) ?? "localhost";
        services.AddScoped<T>(s => GrpcChannel.ForAddress($"http://{ip}:{(short)serviceType}", new GrpcChannelOptions
        {
            MaxRetryAttempts = 3,
            MaxSendMessageSize = null,
            MaxReceiveMessageSize = null,
            LoggerFactory = s.GetRequiredService<ILoggerFactory>(),
            ServiceProvider = s
        }).CreateGrpcService<T>());
    }
    
    public static IServiceCollection AddGrpcDatabaseServices(this IServiceCollection services)
    {
        services.AddGrpcClientService<IAccountService>(GrpcServiceType.DATABASE_SERVICE_HOST);
        return services;
    }
}