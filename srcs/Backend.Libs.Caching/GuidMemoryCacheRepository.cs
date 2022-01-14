using Microsoft.Extensions.Caching.Memory;

namespace Backend.Libs.Caching;

public class GuidMemoryCacheRepository<T> : GenericMemoryCacheRepository<Guid, T> where T : new()
{
    public GuidMemoryCacheRepository(IMemoryCache memoryCache) : base(memoryCache)
    {
    }
}