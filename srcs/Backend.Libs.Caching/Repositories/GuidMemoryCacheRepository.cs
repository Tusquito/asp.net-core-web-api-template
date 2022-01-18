using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Backend.Libs.Caching.Repositories;
public class GuidMemoryCacheRepository<TObject> : GenericMemoryCacheRepository<Guid, TObject> where TObject : new()
{
    public GuidMemoryCacheRepository(IMemoryCache memoryCache, IOptions<MemoryCacheOptions> options) 
        : base(memoryCache, options)
    {
    }
}