using Backend.Libs.gRPC;
using Foundatio.Caching;
using Foundatio.Serializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Backend.Libs.Redis.Extensions;

public static class ServiceCollectionExtensions
{
        private static IConnectionMultiplexer GetConnectionMultiplexer(this IConfiguration configuration) =>
            ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD"),
                EndPoints = { 
                    string.IsNullOrEmpty(configuration.GetConnectionString(GrpcServicesNames.Redis))
                        ? "http://localhost:5672"
                        : configuration.GetConnectionString(GrpcServicesNames.Redis)
                }
            });

        private static void TryAddConnectionMultiplexer(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton(_ => configuration.GetConnectionMultiplexer());
        }

        private static void TryAddRedisCacheClient(this IServiceCollection services)
        {
            services.TryAddSingleton(s => new RedisCacheClient(new RedisCacheClientOptions
            {
                ConnectionMultiplexer = s.GetRequiredService<IConnectionMultiplexer>(),
                Serializer = new JsonNetSerializer()
            }));
            services.TryAddSingleton<ICacheClient>(s => s.GetRequiredService<RedisCacheClient>());
        }
        
        public static void TryAddRedisKeyValueStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddConnectionMultiplexer(configuration);
            services.TryAddRedisCacheClient();
            services.TryAddSingleton(typeof(IKeyValueAsyncStorage<,>), typeof(RedisGenericKeyValueAsyncStorage<,>));
        }
}