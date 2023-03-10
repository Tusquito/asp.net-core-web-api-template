using Backend.Libs.Application.Validators.Options;
using Backend.Libs.Infrastructure.Options;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLibs(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(assembly);
        });
        
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }

    public static IServiceCollection AddOptionsValidatorsLibsOnly(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RabbitMqOptions>, RabbitMqOptionsValidator>();
        services.AddScoped<IValidator<RedisOptions>, RedisOptionsValidator>();
        return services;
    }
}