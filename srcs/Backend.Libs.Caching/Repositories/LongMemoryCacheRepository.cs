using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Backend.Libs.Caching.Repositories;

public class LongMemoryCacheRepository<TObject> : GenericMemoryCacheRepository<long, TObject> where TObject : new()
{
    public LongMemoryCacheRepository(IMemoryCache memoryCache, IOptions<MemoryCacheOptions> options) 
        : base(memoryCache, options)
    {
    }
}