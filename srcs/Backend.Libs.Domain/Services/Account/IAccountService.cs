using System;
using System.Threading;
using System.Threading.Tasks;
using Backend.Libs.Database.Account;

namespace Backend.Libs.Domain.Services.Account;

public interface IAccountService : IRedisService<AccountDto, Guid>
{
    Task<Result<AccountDto?>> GetByEmailAsync(string email, CancellationToken cancellationToken, bool forceRefresh = false);
    Task<Result<AccountDto?>> GetByUsernameAsync(string username, CancellationToken cancellationToken, bool forceRefresh = false);
}