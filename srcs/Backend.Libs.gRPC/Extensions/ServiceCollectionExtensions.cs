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
        services.AddScoped<T>(s =>
        {
            string ip = Environment.GetEnvironmentVariable(serviceType.ToString()) ?? "localhost";
            int port = Convert.ToInt32(serviceType);
            return GrpcChannel.ForAddress($"http://{ip}:{port}", new GrpcChannelOptions
            {
                MaxReceiveMessageSize = null,
                MaxSendMessageSize = null,
                MaxRetryAttempts = 3,
                LoggerFactory = s.GetRequiredService<ILoggerFactory>()
            }).CreateGrpcService<T>();
        });
    }
    
    public static IServiceCollection AddGrpcDatabaseServices(this IServiceCollection services)
    {
        services.AddGrpcClientService<IGrpcAccountService>(GrpcServiceType.DATABASE_SERVER_HOST);
        return services;
    }
}