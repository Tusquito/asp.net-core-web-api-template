using Backend.Libs.Grpc.Account;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.gRPC.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DefaultIp = "localhost";
    private const string DefaultPort = "99999";
    
    private static void AddGrpcClientService<T>(this IServiceCollection services, string serverIpEnv, string serverPortEnv, string? portDefault = null) where T : class
    {
        services.AddGrpcClient<T>(o =>
        {
            string ip = Environment.GetEnvironmentVariable(serverIpEnv) ?? DefaultIp;
            int port = Convert.ToInt32(Environment.GetEnvironmentVariable(serverPortEnv) ?? portDefault ?? DefaultPort);
            
            o.ChannelOptionsActions.Add(co =>
            {
                co.MaxReceiveMessageSize = null;
                co.MaxSendMessageSize = null;
                co.MaxRetryAttempts = 3;
            });

            o.Address = new Uri($"http://{ip}:{port}");
        });
    }
    
    public static IServiceCollection AddGrpcDatabaseServerClients(this IServiceCollection services)
    {
        const string defaultPort = "19999";
        services.AddGrpcClientService<GrpcAccountService.GrpcAccountServiceClient>(
            nameof(GrpcEnvironmentType.GRPC_ACCOUNT_IP), nameof(GrpcEnvironmentType.GRPC_ACCOUNT_PORT), defaultPort);
        return services;
    }
}