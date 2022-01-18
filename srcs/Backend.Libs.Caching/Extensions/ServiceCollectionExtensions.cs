using Backend.Domain.Account;
using Backend.Libs.Caching.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.Caching.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMemoryCachingLib(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.Configure<MemoryCacheOptions>(configuration.GetSection(MemoryCacheOptions.Name));
        services.AddTransient<GuidMemoryCacheRepository<AccountDto>>();
        return services;
    }
}