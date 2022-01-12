using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Api.Database.Generic;

public interface IGenericAsyncRepository<TObject, in TObjectId> 
    where TObject : class
{
    Task<IEnumerable<TObject>> GetAllAsync();

    Task<TObject> GetByIdAsync(TObjectId id);

    Task<IEnumerable<TObject>> GetByIdsAsync(IEnumerable<TObjectId> ids);

    Task<TObject> SaveAsync(TObject obj);

    Task<IEnumerable<TObject>> SaveAsync(IReadOnlyList<TObject> objs);

    Task DeleteByIdAsync(TObjectId id);

    Task DeleteByIdsAsync(IEnumerable<TObjectId> ids);
}