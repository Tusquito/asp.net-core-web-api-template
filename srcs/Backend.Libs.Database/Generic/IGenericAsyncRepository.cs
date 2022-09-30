namespace Backend.Libs.Database.Generic;

public interface IGenericAsyncRepository<TObject, in TObjectId> 
    where TObject : class
{
    Task<List<TObject?>?> GetAllAsync(CancellationToken cancellationToken);

    Task<TObject?> GetByIdAsync(TObjectId id, CancellationToken cancellationToken);

    Task<List<TObject?>?> GetByIdsAsync(IEnumerable<TObjectId> ids, CancellationToken cancellationToken);
    
    Task<TObject?> AddAsync(TObject obj, CancellationToken cancellationToken);

    Task<List<TObject>?> AddRangeAsync(IEnumerable<TObject> objs, CancellationToken cancellationToken);

    Task<TObject?> UpdateAsync(TObject obj, CancellationToken cancellationToken);

    Task<List<TObject>?> UpdateRangeAsync(IEnumerable<TObject> objs, CancellationToken cancellationToken);

    Task DeleteByIdAsync(TObjectId id, CancellationToken cancellationToken);

    Task DeleteByIdsAsync(IEnumerable<TObjectId> ids, CancellationToken cancellationToken);
}