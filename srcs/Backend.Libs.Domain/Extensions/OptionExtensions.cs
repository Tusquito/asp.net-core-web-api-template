using Backend.Libs.Domain.Options;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Backend.Libs.Domain.Extensions;

public static class OptionExtensions
{
    public static IServiceCollection TryConfigure<T>(this IServiceCollection services, IConfiguration configuration) 
        where T : class
    {
        if (services.Any(d => d.ServiceType == typeof(IConfigureOptions<T>)))
        {
            return services;
        }
        
        services.AddOptions<T>()
            .Bind(configuration.GetSection(nameof(T)))
            .ValidateFluently()
            .ValidateOnStart();
        return services;
    }
    
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(s => new FluentValidationOptions<TOptions>(optionsBuilder.Name, s.GetRequiredService<IValidator<TOptions>>()));
        return optionsBuilder;
    }
}