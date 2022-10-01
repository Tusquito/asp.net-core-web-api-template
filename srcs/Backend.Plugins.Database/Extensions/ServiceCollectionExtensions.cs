using Backend.Libs.Database;
using Backend.Libs.Database.Account;
using Backend.Libs.Database.Generic;
using Backend.Plugins.Database.Context;
using Backend.Plugins.Database.Entities;
using Backend.Plugins.Database.Mapping;
using Backend.Plugins.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Backend.Plugins.Database.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddMappedAsyncUuidRepository<TEntity, TDto>(this IServiceCollection services)
        where TEntity : class, IUuidEntity
        where TDto : class, IUuidDTO
    {
        services.AddTransient<IGenericMapper<TEntity, TDto>, GenericMapsterMapper<TEntity, TDto>>();
        services.AddTransient<IGenericAsyncUuidRepository<TDto>, GenericMappedAsyncUuidRepository<TEntity, TDto>>();
        return services;
    }
    
    public static IServiceCollection AddPgsqlDatabaseContext<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddTransient(_ => PgsqlDatabaseConfiguration.FromEnv());
        
        services.AddDbContextFactory<TDbContext>((provider, builder)  =>
        {
            string dbConfig = provider.GetRequiredService<PgsqlDatabaseConfiguration>().ToString();
            builder
                .UseNpgsql(dbConfig, providerOptions =>
                {
                    providerOptions.EnableRetryOnFailure();
                })
                .ConfigureWarnings(s => s.Log(
                    (RelationalEventId.CommandExecuting, LogLevel.Debug),
                    (RelationalEventId.CommandExecuted, LogLevel.Debug)
                ));
        });
        
        services.AddDbContext<TDbContext>((provider, builder)  =>
        {
            string dbConfig = provider.GetRequiredService<PgsqlDatabaseConfiguration>().ToString();
            builder
                .UseNpgsql(dbConfig, providerOptions =>
                {
                    providerOptions.EnableRetryOnFailure();
                })
                .ConfigureWarnings(s => s.Log(
                    (RelationalEventId.CommandExecuting, LogLevel.Debug),
                    (RelationalEventId.CommandExecuted, LogLevel.Debug)
                ));
        });

        return services;
    }

    public static IServiceCollection AddBackendDatabasePlugin(this IServiceCollection services)
    {
        services.AddPgsqlDatabaseContext<BackendDbContext>();

        services.TryAddMappedAsyncUuidRepository<AccountEntity, AccountDTO>();
        services.TryAddTransient<IAccountRepository, AccountRepository>();

        return services;
    }
}