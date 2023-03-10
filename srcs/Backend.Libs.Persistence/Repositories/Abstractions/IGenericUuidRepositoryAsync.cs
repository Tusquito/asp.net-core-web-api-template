using Backend.Libs.Persistence.Data.Abstractions;
using Backend.Libs.Persistence.Entities.Abstractions;

namespace Backend.Libs.Persistence.Repositories.Abstractions;

public interface IGenericUuidRepositoryAsync<TEntity, TDto> : IGenericAsyncRepository<TEntity, TDto, Guid> 
    where TDto : class, IUuidDto
    where TEntity : class, IUuidEntity
{ }