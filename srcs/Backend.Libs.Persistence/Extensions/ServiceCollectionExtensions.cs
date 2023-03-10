using Backend.Libs.Persistence.Context;
using Backend.Libs.Persistence.Data.Abstractions;
using Backend.Libs.Persistence.Data.Account;
using Backend.Libs.Persistence.Entities;
using Backend.Libs.Persistence.Entities.Abstractions;
using Backend.Libs.Persistence.Generic;
using Backend.Libs.Persistence.Mapping;
using Backend.Libs.Persistence.Mapping.Abstractions;
using Backend.Libs.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.Libs.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection TryAddMappedAsyncUuidRepository<TEntity, TDto>(this IServiceCollection services)
        where TEntity : class, IUuidEntity
        where TDto : class, IUuidDto
    {
        services.AddTransient<IGenericMapper<TEntity, TDto>, Mapper<TEntity, TDto>>();
        services.AddTransient<IGenericUuidRepositoryAsync<TEntity, TDto>, GenericUuidRepositoryAsync<TEntity, TDto>>();
        return services;
    }

    private static IServiceCollection AddPgsqlDatabaseContext<TDbContext>(this IServiceCollection services)
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

    public static IServiceCollection AddPersistenceLibs(this IServiceCollection services)
    {
        services.AddPgsqlDatabaseContext<BackendDbContext>();

        services.TryAddMappedAsyncUuidRepository<AccountEntity, AccountDto>();

        return services;
    }
}