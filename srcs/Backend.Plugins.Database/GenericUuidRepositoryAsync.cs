using System.Linq.Expressions;
using Backend.Libs.Database.Abstractions;
using Backend.Plugins.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Backend.Plugins.Database;

public class GenericUuidRepositoryAsync<TEntity, TDto> : IGenericUuidRepositoryAsync<TEntity, TDto>
    where TEntity : class, IUuidEntity
    where TDto : class, IUuidDto

{
    private readonly IGenericMapper<TEntity, TDto?> _mapper;
    private readonly IDbContextFactory<BackendDbContext> _contextFactory;
    private readonly ILogger<GenericUuidRepositoryAsync<TEntity, TDto>> _logger;
        
    public GenericUuidRepositoryAsync(ILogger<GenericUuidRepositoryAsync<TEntity, TDto>> logger, IDbContextFactory<BackendDbContext> contextFactory, IGenericMapper<TEntity, TDto?> mapper)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task<TDto?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return _mapper.Map(await context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(FirstOrDefaultAsync));
            return default;
        }
    }

    public async Task<List<TDto?>?> WhereAsync(Expression<Func<TEntity,bool>> predicate, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return _mapper.Map(await context.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(WhereAsync));
            return default;
        }
    }

    public async Task<int> BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return await context.Set<TEntity>().Where(predicate).ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(BulkDeleteAsync));
            return -1;
        }
    }

    public async Task<int> BulkUpdateAsync(Expression<Func<SetPropertyCalls<TEntity>,SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return await context.Set<TEntity>().ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(BulkUpdateAsync));
            return -1;
        }
    }

    public async Task<List<TDto?>?> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return _mapper.Map(await context.Set<TEntity>().ToListAsync(cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetAllAsync));
            return default;
        }
    }

    public async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return _mapper.Map(await context.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetByIdAsync));
            return default;
        }
    }

    public async Task<List<TDto?>?> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return _mapper.Map(await context.Set<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetByIdsAsync));
            return default;
        }
    }
    
    public async Task<TDto?> AddAsync(TDto obj, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            TEntity? map = _mapper.Map(obj);
            if (map == null)
            {
                return default;
            }
            EntityEntry<TEntity> tmp = await context.Set<TEntity>().AddAsync(map, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return _mapper.Map(tmp.Entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(AddAsync));
            return default;
        }
    }

    public async Task<List<TDto>?> AddRangeAsync(IEnumerable<TDto> objs, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            TDto[] array = objs as TDto[] ?? objs.ToArray();
            IReadOnlyList<TEntity>? tmp = _mapper.Map(array);
            if (tmp == null)
            {
                return new List<TDto>();
            }
            await context.Set<TEntity>().AddRangeAsync(tmp, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return array.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(AddRangeAsync));
            return default;
        }
    }

    public async Task<TDto?> UpdateAsync(TDto obj, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            TEntity? map = _mapper.Map(obj);
            if (map == null)
            {
                return default;
            }
            EntityEntry<TEntity> tmp = context.Set<TEntity>().Update(map);
            await context.SaveChangesAsync(cancellationToken);
            return _mapper.Map(tmp.Entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(UpdateAsync));
            return default;
        }
    }

    public async Task<List<TDto>?> UpdateRangeAsync(IEnumerable<TDto> objs, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            TDto[] array = objs as TDto[] ?? objs.ToArray();
            IReadOnlyList<TEntity>? tmp = _mapper.Map(array);
            if (tmp == null)
            {
                return new List<TDto>();
            }
            context.Set<TEntity>().UpdateRange(tmp);
            await context.SaveChangesAsync(cancellationToken);
            return array.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(UpdateRangeAsync));
            return default;
        }
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            TEntity? tmp = await context.Set<TEntity>().FindAsync(id);
            if (tmp == null)
            {
                return;
            }
            context.Set<TEntity>().Remove(tmp);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(DeleteByIdAsync));
        }
    }

    public async Task DeleteByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            context.Set<TEntity>().RemoveRange(context.Set<TEntity>().Where(x => ids.Contains(x.Id)));
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(DeleteByIdsAsync));
        }
    }
}