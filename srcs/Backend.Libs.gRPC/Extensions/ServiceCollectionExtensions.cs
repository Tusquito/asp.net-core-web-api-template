using Backend.Libs.gRPC.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.gRPC.Extensions;

public static class ServiceCollectionExtensions
{
    private static void AddGrpcClientService<T>(this IServiceCollection services, string serviceName, IConfiguration configuration, int defaultPort) where T : class
    {
        services.AddGrpcClient<T>(o =>
        {
            o.ChannelOptionsActions.Add(co =>
            {
                co.MaxReceiveMessageSize = null;
                co.MaxSendMessageSize = null;
                co.MaxRetryAttempts = 3;
            });

            o.Address = configuration.GetServiceUri(serviceName) ?? new Uri($"http://localhost:{defaultPort}");
        });
    }
    
    public static IServiceCollection AddGrpcDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClientService<GrpcAccountService.GrpcAccountServiceClient>(GrpcServicesNames.Database, configuration, 7771);
        return services;
    }
}