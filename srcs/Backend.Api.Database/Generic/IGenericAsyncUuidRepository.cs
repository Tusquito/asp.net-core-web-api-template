using System;
using Backend.Domain;

namespace Backend.Api.Database.Generic
{
    public interface IGenericAsyncUuidRepository<TDto> : IGenericAsyncRepository<TDto, Guid> 
        where TDto : class, IUuidDto
    { }
}