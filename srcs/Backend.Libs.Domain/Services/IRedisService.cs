using System.Threading;
using System.Threading.Tasks;
using Backend.Plugins.Domain;

namespace Backend.Libs.Domain.Services;

public interface IRedisService<TObject, in TKey>
{
    Task<ServiceResult<TObject?>> GetByIdAsync(TKey id, CancellationToken cancellationToken, bool forceRefresh = false);
    Task<ServiceResult> UpdateAsync(TObject obj, CancellationToken cancellationToken);
    Task<ServiceResult<TObject?>> AddAsync(TObject obj, CancellationToken cancellationToken);
    Task<ServiceResult> DeleteByIdAsync(TKey id, CancellationToken cancellationToken);
}