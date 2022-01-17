using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Backend.Libs.Caching;

public class GenericMemoryCacheRepository<TKey, TObject> : IGenericMemoryCacheRepository<TKey, TObject> where TObject : new()
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheOptions _memoryCacheOptions;

    public GenericMemoryCacheRepository(IMemoryCache memoryCache, IOptions<MemoryCacheOptions> memoryCacheOptions)
    {
        _memoryCache = memoryCache;
        _memoryCacheOptions = memoryCacheOptions.Value;
    }
    public TObject Get(TKey key)
    {
        return _memoryCache.Get<TObject>(ToCacheKey(key));
    }

    public bool Exists(TKey key)
    {
        return _memoryCache.TryGetValue(key, out _);
    }

    public IEnumerable<TObject> GetMany(TKey key)
    {
        return _memoryCache.Get<IEnumerable<TObject>>(ToCacheKey(key));
    }

    public void SetOrCreate(TKey key, TObject obj)
    {
        _memoryCache.Set(ToCacheKey(key), obj, new MemoryCacheEntryOptions
        {
            Priority = CacheItemPriority.Normal,
            AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration, 
            SlidingExpiration = _memoryCacheOptions.SlidingExpiration
        });
    }

    public void SetOrCreate(TKey key, List<TObject> obj)
    {
        _memoryCache.Set(ToCacheKey(key), obj, new MemoryCacheEntryOptions
        {
            Priority = CacheItemPriority.Normal,
            AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration, 
            SlidingExpiration = _memoryCacheOptions.SlidingExpiration
        });
    }

    public void Remove(TKey key)
    {
        _memoryCache.Remove(ToCacheKey(key));
    }

    public async Task<TObject> GetOrCreateAsync(TKey key, Func<Task<TObject>> func)
    {
        return await _memoryCache.GetOrCreateAsync(ToCacheKey(key), async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return await func();
        });
    }

    public async Task<IEnumerable<TObject>> GetOrCreateAsync(TKey key, Func<Task<IEnumerable<TObject>>> func)
    {
        return await _memoryCache.GetOrCreateAsync(ToCacheKey(key), async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return await func();
        });
    }

    public async Task<TObject> GetOrCreateAsync(TKey key, Func<TObject> func)
    {
        return await _memoryCache.GetOrCreateAsync(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return Task.FromResult(func());
        });
    }

    public async Task<IEnumerable<TObject>> GetOrCreateAsync(TKey key, Func<IEnumerable<TObject>> func)
    {
        return await _memoryCache.GetOrCreateAsync(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return Task.FromResult(func());
        });
    }

    public async Task<TObject> GetOrCreateAsync(TKey key, TObject obj)
    {
        return await _memoryCache.GetOrCreateAsync(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return Task.FromResult(obj);
        });
    }

    public async Task<IEnumerable<TObject>> GetOrCreateAsync(TKey key, IEnumerable<TObject> obj)
    {
        return await _memoryCache.GetOrCreateAsync(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return Task.FromResult(obj);
        });
    }

    public TObject GetOrCreate(TKey key, Func<TObject> func)
    {
        return _memoryCache.GetOrCreate(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return func();
        });
    }

    public IEnumerable<TObject> GetOrCreate(TKey key, Func<IEnumerable<TObject>> func)
    {
        return _memoryCache.GetOrCreate(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return func();
        });
    }

    public TObject GetOrCreate(TKey key, TObject obj)
    {
        return _memoryCache.GetOrCreate(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return obj;
        });
    }

    public IEnumerable<TObject> GetOrCreate(TKey key, IEnumerable<TObject> obj)
    {
        return _memoryCache.GetOrCreate(ToCacheKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _memoryCacheOptions.AbsoluteExpiration;
            entry.SlidingExpiration = _memoryCacheOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return obj;
        });
    }

    private string ToCacheKey(TKey key)
    {
        if (key == null)
        {
            throw new ArgumentException(nameof(key));
        }

        string? keyToString = Convert.ToString(key);

        if (string.IsNullOrWhiteSpace(keyToString))
        {
            throw new ArgumentException($"Can not convert {typeof(TKey)} to string");
        }
        
        return $"{_memoryCacheOptions.RepositoryBaseKey}{keyToString.Trim().Replace(' ', '-')}";
    }
}