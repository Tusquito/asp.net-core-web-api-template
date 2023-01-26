using Backend.Libs.Database.Generic;
using Backend.Plugins.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Backend.Plugins.Database;

public class GenericMappedAsyncUuidRepository<TEntity, TDto> : IGenericAsyncUuidRepository<TDto>
    where TEntity : class, IUuidEntity
    where TDto : class, IUuidDto

{
    private readonly IGenericMapper<TEntity, TDto?> _mapper;
    private readonly IDbContextFactory<BackendDbContext> _contextFactory;
    private readonly ILogger<GenericMappedAsyncUuidRepository<TEntity, TDto>> _logger;
        
    public GenericMappedAsyncUuidRepository(ILogger<GenericMappedAsyncUuidRepository<TEntity, TDto>> logger, IDbContextFactory<BackendDbContext> contextFactory, IGenericMapper<TEntity, TDto?> mapper)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _mapper = mapper;
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
            return new List<TDto?>();
        }
    }

    public async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return _mapper.Map(await context.Set<TEntity>().FindAsync(id));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetByIdAsync));
            return null;
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
            return new List<TDto?>();
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
                return null;
            }
            EntityEntry<TEntity> tmp = await context.Set<TEntity>().AddAsync(map, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return _mapper.Map(tmp.Entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(AddAsync));
            return null;
        }
    }

    public async Task<List<TDto>?> AddRangeAsync(IEnumerable<TDto> objs, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            TDto[] uuidDtos = objs as TDto[] ?? objs.ToArray();
            IReadOnlyList<TEntity>? tmp = _mapper.Map(uuidDtos);
            if (tmp == null)
            {
                return new List<TDto>();
            }
            await context.Set<TEntity>().AddRangeAsync(tmp, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return uuidDtos.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(AddRangeAsync));
            return new List<TDto>();
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
                return null;
            }
            EntityEntry<TEntity> tmp = context.Set<TEntity>().Update(map);
            await context.SaveChangesAsync(cancellationToken);
            return _mapper.Map(tmp.Entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(UpdateAsync));
            return null;
        }
    }

    public async Task<List<TDto>?> UpdateRangeAsync(IEnumerable<TDto> objs, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            TDto[] uuidDtos = objs as TDto[] ?? objs.ToArray();
            IReadOnlyList<TEntity>? tmp = _mapper.Map(uuidDtos);
            if (tmp == null)
            {
                return new List<TDto>();
            }
            context.Set<TEntity>().UpdateRange();
            await context.SaveChangesAsync(cancellationToken);
            return uuidDtos.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(UpdateRangeAsync));
            return new List<TDto>();
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