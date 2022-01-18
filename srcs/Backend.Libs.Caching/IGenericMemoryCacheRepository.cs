namespace Backend.Libs.Caching;

public interface IGenericMemoryCacheRepository<in TKey, TObject>
{
    TObject Get(TKey key);
    bool Exists(TKey key);
    IEnumerable<TObject> GetMany(TKey key);
    void SetOrCreate(TKey key, TObject obj);
    void SetOrCreate(TKey key, IEnumerable<TObject> objs);
    void Remove(TKey key);
    Task<TObject> SetOrCreateAsync(TKey key, Func<Task<TObject>> func);    
    Task<IEnumerable<TObject>> SetOrCreateAsync(TKey key, Func<Task<IEnumerable<TObject>>> func);
    Task<TObject> SetOrCreateAsync(TKey key, Func<TObject> func);
    Task<IEnumerable<TObject>> SetOrCreateAsync(TKey key, Func<IEnumerable<TObject>> func);
    Task<TObject> SetOrCreateAsync(TKey key, TObject obj);
    Task<IEnumerable<TObject>> SetOrCreateAsync(TKey key, IEnumerable<TObject> objs);
    TObject SetOrCreate(TKey key, Func<TObject> func);
    IEnumerable<TObject> SetOrCreate(TKey key, Func<IEnumerable<TObject>> func);
}