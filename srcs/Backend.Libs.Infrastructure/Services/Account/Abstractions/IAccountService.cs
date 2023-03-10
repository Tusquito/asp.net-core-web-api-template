using Backend.Libs.Domain;
using Backend.Libs.Domain.Services.Caching.Abstractions;
using Backend.Libs.Persistence.Data.Account;

namespace Backend.Libs.Infrastructure.Services.Account.Abstractions;

public interface IAccountService : IRedisService<AccountDto, Guid>
{
    Task<Result<AccountDto>> GetByEmailAsync(string email, CancellationToken cancellationToken, bool forceRefresh = false);
    Task<Result<AccountDto>> GetByUsernameAsync(string username, CancellationToken cancellationToken, bool forceRefresh = false);
}