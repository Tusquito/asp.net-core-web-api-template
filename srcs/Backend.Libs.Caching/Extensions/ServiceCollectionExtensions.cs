using Backend.Libs.Caching.Serializers;
using Foundatio.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Backend.Libs.Caching.Extensions;

public static class ServiceCollectionExtensions
{
        private static IConnectionMultiplexer GetConnectionMultiplexer(this RedisConfiguration configuration) =>
            ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                Password = configuration.Password,
                EndPoints = { configuration.ToString() }
            });

        private static void TryAddConfigurationFromEnv(this IServiceCollection services) => services.TryAddSingleton(_ => RedisConfiguration.FromEnv());

        private static void TryAddConnectionMultiplexer(this IServiceCollection services) => services.TryAddSingleton(s => s.GetRequiredService<RedisConfiguration>().GetConnectionMultiplexer());


        private static void TryAddConnectionMultiplexerFromEnv(this IServiceCollection services)
        {
            services.TryAddConfigurationFromEnv();
            services.TryAddConnectionMultiplexer();
        }

        private static void TryAddRedisCacheClient(this IServiceCollection services)
        {
            services.TryAddSingleton(s => new RedisCacheClient(new RedisCacheClientOptions
            {
                ConnectionMultiplexer = s.GetRequiredService<IConnectionMultiplexer>(),
                Serializer = new ProtoSerializer()
            }));
            services.TryAddSingleton<ICacheClient>(s => s.GetRequiredService<RedisCacheClient>());
        }
        
        public static void TryAddRedisKeyValueStorage(this IServiceCollection services)
        {
            services.TryAddConnectionMultiplexerFromEnv();
            services.TryAddRedisCacheClient();
            services.TryAddSingleton(typeof(IKeyValueAsyncStorage<,>), typeof(RedisGenericKeyValueAsyncStorage<,>));
        }
}