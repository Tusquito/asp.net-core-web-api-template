using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Api.Database.Context;
using Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Api.Database.Generic
{
    public class GenericMappedAsyncUuidRepository<TEntity, TDto> : IGenericAsyncUuidRepository<TDto>
        where TEntity : class, IUuidEntity
        where TDto : class, IUuidDto

    {
        private readonly IGenericMapper<TEntity, TDto> _mapper;
        private readonly IDbContextFactory<WebContext> _contextFactory;
        private readonly ILogger<GenericMappedAsyncUuidRepository<TEntity, TDto>> _logger;
        
        public GenericMappedAsyncUuidRepository(ILogger<GenericMappedAsyncUuidRepository<TEntity, TDto>> logger, IDbContextFactory<WebContext> contextFactory, IGenericMapper<TEntity, TDto> mapper)
        {
            _logger = logger;
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return _mapper.Map(await context.Set<TEntity>().ToListAsync());
            }
            catch (Exception e)
            {
                _logger.LogError("GetAllAsync " + e.Message);
                return Array.Empty<TDto>();
            }
        }

        public async Task<TDto> GetByIdAsync(Guid id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return _mapper.Map(await context.Set<TEntity>().FindAsync(id));
            }
            catch (Exception e)
            {
                _logger.LogError("GetByIdAsync " + e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<TDto>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return _mapper.Map(await context.Set<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync());
            }
            catch (Exception e)
            {
                _logger.LogError("GetByIdsAsync " + e.Message);
                return Array.Empty<TDto>();
            }
        }

        public async Task<TDto> SaveAsync(TDto obj)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                var tmp = context.Set<TEntity>().Update(_mapper.Map(obj));
                await context.SaveChangesAsync();
                return _mapper.Map(tmp.Entity);
            }
            catch (Exception e)
            {
                _logger.LogError("SaveAsync " + e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<TDto>> SaveAsync(IReadOnlyList<TDto> objs)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                context.Set<TEntity>().UpdateRange(_mapper.Map(objs));
                await context.SaveChangesAsync();
                return objs;
            }
            catch (Exception e)
            {
                _logger.LogError("SaveAsync " + e.Message);
                return Array.Empty<TDto>();
            }
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                context.Set<TEntity>().Remove(await context.Set<TEntity>().FindAsync(id));
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("DeleteByIdAsync " + e.Message);
            }
        }

        public async Task DeleteByIdsAsync(IEnumerable<Guid> ids)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                context.Set<TEntity>().RemoveRange(context.Set<TEntity>().Where(x => ids.Contains(x.Id)));
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("DeleteByIdsAsync " + e.Message);
            }
        }
    }
}