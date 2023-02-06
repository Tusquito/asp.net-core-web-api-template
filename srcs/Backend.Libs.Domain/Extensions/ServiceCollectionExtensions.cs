using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainLibs(this IServiceCollection services)
    {
        // Fluent Validation
        services.AddValidatorsFromAssemblyContaining(typeof(ServiceCollectionExtensions));
        
        // MediatR
        services.AddMediatR(typeof(ServiceCollectionExtensions));
        
        return services;
    }
}