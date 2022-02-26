using Foundatio.Caching;
using Foundatio.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Backend.Libs.Redis.Extensions;

public static class ServiceCollectionExtensions
{
        public static IConnectionMultiplexer GetConnectionMultiplexer(this RedisConfiguration redisConfiguration) =>
            ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                Password = redisConfiguration.Password,
                EndPoints = { $"{redisConfiguration.Host}:{redisConfiguration.Port}" }
            });
    
        public static void TryAddConfigurationFromEnv(this IServiceCollection services)
        {
            services.TryAddSingleton(s => RedisConfiguration.FromEnv());
        }
        
        internal static void TryAddConnectionMultiplexer(this IServiceCollection services)
        {
            services.TryAddSingleton(s => s.GetService<RedisConfiguration>().GetConnectionMultiplexer());
        }

        /// <summary>
        ///     Registers the Connection Multiplexer
        /// </summary>
        /// <param name="services"></param>
        public static void TryAddConnectionMultiplexerFromEnv(this IServiceCollection services)
        {
            services.TryAddConfigurationFromEnv();
            services.TryAddConnectionMultiplexer();
        }

        public static void TryAddRedisCacheClient(this IServiceCollection services)
        {
            services.TryAddSingleton(s => new RedisCacheClient(new RedisCacheClientOptions
            {
                ConnectionMultiplexer = s.GetRequiredService<IConnectionMultiplexer>(),
                Serializer = new JsonNetSerializer()
            }));
            services.TryAddSingleton<ICacheClient>(s => s.GetRequiredService<RedisCacheClient>());
        }
        
        public static void TryAddRedisKeyValueStorageFromEnv(this IServiceCollection services)
        {
            services.TryAddConnectionMultiplexerFromEnv();
            services.TryAddRedisCacheClient();
            services.TryAddSingleton(typeof(IKeyValueAsyncStorage<,>), typeof(RedisGenericKeyValueAsyncStorage<,>));
        }
}