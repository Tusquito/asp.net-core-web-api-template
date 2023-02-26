namespace Backend.Libs.Database.Abstractions;

public interface IGenericUuidRepositoryAsync<TEntity, TDto> : IGenericAsyncRepository<TEntity, TDto, Guid> 
    where TDto : class, IUuidDto
    where TEntity : class, IUuidEntity
{ }