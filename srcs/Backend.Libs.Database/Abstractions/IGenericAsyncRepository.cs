using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Backend.Libs.Database.Abstractions;

public interface IGenericAsyncRepository<TEntity, TDto, in TId> 
    where TDto : class, IDto
    where TEntity : class, IEntity
{

    Task<TDto?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<List<TDto?>?> WhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<int> BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<int> BulkUpdateAsync(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        CancellationToken cancellationToken);
    Task<List<TDto?>?> GetAllAsync(CancellationToken cancellationToken);

    Task<TDto?> GetByIdAsync(TId id, CancellationToken cancellationToken);

    Task<List<TDto?>?> GetByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken);
    
    Task<TDto?> AddAsync(TDto obj, CancellationToken cancellationToken);

    Task<List<TDto>?> AddRangeAsync(IEnumerable<TDto> objs, CancellationToken cancellationToken);

    Task<TDto?> UpdateAsync(TDto obj, CancellationToken cancellationToken);

    Task<List<TDto>?> UpdateRangeAsync(IEnumerable<TDto> objs, CancellationToken cancellationToken);

    Task DeleteByIdAsync(TId id, CancellationToken cancellationToken);

    Task DeleteByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken);
}