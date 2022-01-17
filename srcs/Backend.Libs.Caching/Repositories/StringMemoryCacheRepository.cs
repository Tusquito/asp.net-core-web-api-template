using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Backend.Libs.Caching.Repositories;

public class StringMemoryCacheRepository<TObject> : GenericMemoryCacheRepository<string, TObject> where TObject : new()
{
    public StringMemoryCacheRepository(IMemoryCache memoryCache, IOptions<MemoryCacheOptions> options) 
        : base(memoryCache, options)
    {
    }
}