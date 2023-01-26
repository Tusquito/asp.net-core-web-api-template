using Backend.Libs.Database.Generic;

namespace Backend.Libs.Database.Account;

public interface IAccountRepository : IGenericAsyncUuidRepository<AccountDto>
{
    Task<AccountDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<AccountDto?> GetAccountByIpAsync(string ip, CancellationToken cancellationToken);
    Task<AccountDto?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}