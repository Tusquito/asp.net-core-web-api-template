﻿namespace Backend.Libs.Domain.Services.Caching.Abstractions;

public interface IRedisService<TObject, in TKey>
{
    Task<Result<TObject>> GetByIdAsync(TKey id, CancellationToken cancellationToken, bool forceRefresh = false);
    Task<Result> UpdateAsync(TObject obj, CancellationToken cancellationToken);
    Task<Result<TObject>> AddAsync(TObject obj, CancellationToken cancellationToken);
    Task<Result> DeleteByIdAsync(TKey id, CancellationToken cancellationToken);
}