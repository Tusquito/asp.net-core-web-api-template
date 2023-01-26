using Backend.Libs.Domain.Services.Account;
using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Enums;
using Backend.Plugins.Domain.Services.Account;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;

namespace Backend.Plugins.gRPC.Extensions;

public static class ServiceCollectionExtensions
{
    private static void AddGrpcClientService<T>(this IServiceCollection services, GrpcServiceType serviceType) where T : class
    {
        services.AddScoped<T>(s =>
        {
            string ip = Environment.GetEnvironmentVariable(serviceType.ToString()) ?? "localhost";
            int port = Convert.ToInt32(serviceType);
            CallCredentials credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                // TODO AUTHENTICATE SERVICES BETWEEN THEMSELVES
                return Task.CompletedTask;
            });
            
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
        services.AddGrpcClientService<IGrpcAccountService>(GrpcServiceType.DatabaseServerPort);
        return services;
    }
}