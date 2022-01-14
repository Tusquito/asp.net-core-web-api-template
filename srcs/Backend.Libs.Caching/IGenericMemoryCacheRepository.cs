namespace Backend.Libs.Caching;

public interface IGenericMemoryCacheRepository<in TKey, TObject>
{
    TObject Get(TKey key);
    void SetOrCreate(TKey key, TObject obj);
    void Remove(TKey key);
    Task<TObject> GetOrCreateAsync(TKey key, Func<Task<TObject>> func);
    Task<TObject> GetOrCreateAsync(TKey key, Func<TObject> func);
    Task<TObject> GetOrCreateAsync(TKey key, TObject obj);
    TObject GetOrCreate(TKey key, Func<TObject> func);
    TObject GetOrCreate(TKey key, TObject obj);
}