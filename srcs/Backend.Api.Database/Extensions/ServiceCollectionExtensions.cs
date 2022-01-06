using Backend.Api.Database.Account;
using Backend.Api.Database.Generic;
using Backend.Api.Database.Utils;
using Backend.Domain;
using Backend.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Backend.Api.Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddPgsqlDatabaseContext<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            services.Configure<PgsqlDatabaseConfigurationOptions>(configuration.GetSection(PgsqlDatabaseConfigurationOptions.Name));
            services.AddDbContext<TDbContext>((provider, builder)  =>
            {
                string dbConfig = provider.GetRequiredService<IOptions<PgsqlDatabaseConfigurationOptions>>().Value.ToString();
                builder
                    .UseNpgsql(dbConfig)
                    .ConfigureWarnings(s => s.Log(
                        (RelationalEventId.CommandExecuting, LogLevel.Debug),
                        (RelationalEventId.CommandExecuted, LogLevel.Debug)
                    ));
            }, ServiceLifetime.Transient, ServiceLifetime.Singleton);
            

            services.AddDbContextFactory<TDbContext>((provider, builder) =>
            {
                string dbConfig = provider.GetRequiredService<IOptions<PgsqlDatabaseConfigurationOptions>>().Value.ToString();
                builder
                    .UseNpgsql(dbConfig)
                    .ConfigureWarnings(s => s.Log(
                        (RelationalEventId.CommandExecuting, LogLevel.Debug),
                        (RelationalEventId.CommandExecuted, LogLevel.Debug)
                    ));
            });

            return services;
        }
        
        public static IServiceCollection TryAddMappedAsyncUuidRepository<TEntity, TDto>(this IServiceCollection services)
            where TEntity : class, IUuidEntity
            where TDto : class, IUuidDto
        {
            services.AddTransient<IGenericMapper<TEntity, TDto>, MapsterMapper<TEntity, TDto>>();
            services.AddTransient<IGenericAsyncUuidRepository<TDto>, GenericMappedAsyncUuidRepository<TEntity, TDto>>();
            return services;
        }
        
        public static IServiceCollection AddDatabaseRepositories(this IServiceCollection services)
        {
            // Accounts
            services.TryAddMappedAsyncUuidRepository<AccountEntity, AccountDto>();
            services.AddTransient<IAccountDao, AccountDao>();
            return services;
        }
    }
}