namespace Backend.Libs.Database.Generic;

public interface IGenericAsyncRepository<TObject, in TObjectId> 
    where TObject : class
{
    Task<List<TObject>> GetAllAsync();

    Task<TObject?> GetByIdAsync(TObjectId id);

    Task<List<TObject>> GetByIdsAsync(IEnumerable<TObjectId> ids);
    
    Task<TObject?> AddAsync(TObject obj);

    Task<List<TObject>> AddRangeAsync(IEnumerable<TObject> objs);

    Task<TObject?> UpdateAsync(TObject obj);

    Task<List<TObject>> UpdateRangeAsync(IEnumerable<TObject> objs);

    Task DeleteByIdAsync(TObjectId id);

    Task DeleteByIdsAsync(IEnumerable<TObjectId> ids);
}