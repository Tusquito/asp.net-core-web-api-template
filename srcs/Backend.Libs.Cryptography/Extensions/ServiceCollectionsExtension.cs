using Backend.Libs.Cryptography.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.Cryptography.Extensions;

public static class ServiceCollectionsExtension
{
    public static IServiceCollection AddCryptographyLibs(this IServiceCollection services)
    {
        // BcryptPasswordHasher
        services.AddTransient<IPasswordHasherService, BCryptPasswordHasherService>();
        return services;
    }
}