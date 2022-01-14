using Microsoft.Extensions.Caching.Memory;

namespace Backend.Libs.Caching;

public class GenericMemoryCacheRepository<TKey, TObject> : IGenericMemoryCacheRepository<TKey, TObject> where TObject : new()
{
    private const string BaseKey = "backend-cache:";
    private readonly int _memoryCacheLifetimeInHours = Convert.ToInt32(Environment.GetEnvironmentVariable("MEMORY_CACHE_LIFETIME_IN_HOURS") ?? "1");
    private readonly IMemoryCache _memoryCache;
    public GenericMemoryCacheRepository(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public TObject Get(TKey key)
    {
        return _memoryCache.Get<TObject>(ToCacheKey(key));
    }

    public void SetOrCreate(TKey key, TObject obj)
    {
        _memoryCache.Set(ToCacheKey(key), obj, TimeSpan.FromHours(_memoryCacheLifetimeInHours));
    }

    public void Remove(TKey key)
    {
        _memoryCache.Remove(ToCacheKey(key));
    }

    public async Task<TObject> GetOrCreateAsync(TKey key, Func<Task<TObject>> func)
    {
        return await _memoryCache.GetOrCreateAsync(ToCacheKey(key), async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_memoryCacheLifetimeInHours);
            return await func();
        });
    }

    public async Task<TObject> GetOrCreateAsync(TKey key, Func<TObject> func)
    {
        return await _memoryCache.GetOrCreateAsync(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_memoryCacheLifetimeInHours);
            return Task.FromResult(func());
        });
    }

    public async Task<TObject> GetOrCreateAsync(TKey key, TObject obj)
    {
        return await _memoryCache.GetOrCreateAsync(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_memoryCacheLifetimeInHours);
            return Task.FromResult(obj);
        });
    }

    public TObject GetOrCreate(TKey key, Func<TObject> func)
    {
        return _memoryCache.GetOrCreate(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_memoryCacheLifetimeInHours);
            return func();
        });
    }

    public TObject GetOrCreate(TKey key, TObject obj)
    {
        return _memoryCache.GetOrCreate(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_memoryCacheLifetimeInHours);
            return obj;
        });
    }

    private static string ToCacheKey(TKey key)
    {
        if (key is null)
        {
            throw new ArgumentException(nameof(key));
        }
        
        return $"{BaseKey}{key.ToString().Trim().Replace(' ', '-')}";
    }
}