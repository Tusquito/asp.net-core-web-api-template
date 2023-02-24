using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainLibs(this IServiceCollection services)
    {
        // Fluent Validation
        services.AddValidatorsFromAssemblyContaining(typeof(ServiceCollectionExtensions));
        return services;
    }
}