using System.Collections.Generic;

namespace Backend.Api.Database.Generic;

public interface IGenericMapper<TEntity, TDto>
{
    TEntity Map(TDto input);

    List<TEntity> Map(List<TDto> input);

    IEnumerable<TEntity> Map(IEnumerable<TDto> input);

    IReadOnlyList<TEntity> Map(IReadOnlyList<TDto> input);

    TDto Map(TEntity input);

    List<TDto> Map(List<TEntity> input);

    IEnumerable<TDto> Map(IEnumerable<TEntity> input);

    IReadOnlyList<TDto> Map(IReadOnlyList<TEntity> input);

    void Map(TDto input, TEntity output);

    void Map(TEntity input, TDto output);
}