namespace Backend.Libs.Caching;

public interface IGenericMemoryCacheRepository<in TKey, TObject>
{
    TObject Get(TKey key);
    bool Exists(TKey key);
    IEnumerable<TObject> GetMany(TKey key);
    void SetOrCreate(TKey key, TObject obj);
    void SetOrCreate(TKey key, List<TObject> obj);
    void Remove(TKey key);
    Task<TObject> GetOrCreateAsync(TKey key, Func<Task<TObject>> func);    
    Task<IEnumerable<TObject>> GetOrCreateAsync(TKey key, Func<Task<IEnumerable<TObject>>> func);
    Task<TObject> GetOrCreateAsync(TKey key, Func<TObject> func);
    Task<IEnumerable<TObject>> GetOrCreateAsync(TKey key, Func<IEnumerable<TObject>> func);
    Task<TObject> GetOrCreateAsync(TKey key, TObject obj);
    Task<IEnumerable<TObject>> GetOrCreateAsync(TKey key, IEnumerable<TObject> obj);
    TObject GetOrCreate(TKey key, Func<TObject> func);
    IEnumerable<TObject> GetOrCreate(TKey key, Func<IEnumerable<TObject>> func);
    TObject GetOrCreate(TKey key, TObject obj);
    IEnumerable<TObject> GetOrCreate(TKey key, IEnumerable<TObject> obj);
}