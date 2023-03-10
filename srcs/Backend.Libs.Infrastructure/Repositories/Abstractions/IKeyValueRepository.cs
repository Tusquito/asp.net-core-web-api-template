namespace Backend.Libs.Infrastructure.Repositories.Abstractions;

    public interface IKeyValueRepository<TKey, TObject>
    where TKey : notnull
    {
        Task<IEnumerable<TObject>> GetAllAsync();
        Task ClearAllAsync();
        Task<IEnumerable<TObject>> GetByIdsAsync(IEnumerable<TKey> ids);
        Task<TObject?> GetByIdAsync(TKey id);
        Task RegisterAsync(TKey id, TObject obj, TimeSpan? lifeTime = null, bool keepTtl = false);
        Task RegisterAsync(IEnumerable<(TKey, TObject)> objs, TimeSpan? lifeTime = null, bool keepTtl = false);
        Task<TObject?> RemoveAsync(TKey id);
        Task<IEnumerable<TObject>?> RemoveAsync(IEnumerable<TKey> ids);
    }