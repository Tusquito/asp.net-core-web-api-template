using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Backend.Libs.Caching.Repositories;

public class IntMemoryCacheRepository<TObject> : GenericMemoryCacheRepository<int, TObject> where TObject : new()
{
    public IntMemoryCacheRepository(IMemoryCache memoryCache, IOptions<MemoryCacheOptions> options) 
        : base(memoryCache, options)
    {
    }
}
