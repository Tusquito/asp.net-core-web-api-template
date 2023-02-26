using Backend.Libs.Database.Abstractions;
using Mapster;

namespace Backend.Plugins.Database.Mapping;

public class GenericMapsterMapper<TEntity, TDto> : IGenericMapper<TEntity, TDto> 
    where TDto : class, IDto
    where TEntity : class, IEntity
{
    public TEntity? Map(TDto? input)
    {
        return input?.Adapt<TEntity?>();
    }

    public List<TEntity>? Map(List<TDto>? input)
    {
        return input?.Adapt<List<TEntity>?>();
    }

    public IEnumerable<TEntity>? Map(IEnumerable<TDto>? input)
    {
        return input?.Adapt<IEnumerable<TEntity>?>();
    }

    public IReadOnlyList<TEntity>? Map(IReadOnlyList<TDto>? input)
    {
        return input?.Adapt<List<TEntity>?>();
    }

    public TDto? Map(TEntity? input)
    {
        return input?.Adapt<TDto?>();
    }

    public List<TDto>? Map(List<TEntity>? input)
    {
        return input?.Adapt<List<TDto>?>();
    }

    public IEnumerable<TDto>? Map(IEnumerable<TEntity>? input)
    {
        return input?.Adapt<IEnumerable<TDto>>();
    }

    public IReadOnlyList<TDto>? Map(IReadOnlyList<TEntity>? input)
    {
        return input?.Adapt<List<TDto>?>();
    }
}