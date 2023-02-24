using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.Mediator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatorLibs(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(x =>
        {
            x.Lifetime = ServiceLifetime.Scoped;
            x.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });

        return services;
    }
}