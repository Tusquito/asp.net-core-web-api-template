namespace Backend.Libs.Database.Generic;

public interface IGenericAsyncUuidRepository<TDto> : IGenericAsyncRepository<TDto, Guid> 
    where TDto : class, IUuidDTO
{ }