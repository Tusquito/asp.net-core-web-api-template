using Backend.Libs.Domain.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainLibs(this IServiceCollection services)
    {
        // Fluent Validation
        services.AddValidatorsFromAssemblyContaining(typeof(ServiceCollectionExtensions));
        
        // MediatR
        services.AddMediatR(x =>
        {
            x.Lifetime = ServiceLifetime.Scoped;
            x.RegisterServicesFromAssemblyContaining<ICommand>();
        });
        
        return services;
    }
}