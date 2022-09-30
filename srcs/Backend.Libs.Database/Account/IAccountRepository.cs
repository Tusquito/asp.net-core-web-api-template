using Backend.Libs.Database.Generic;

namespace Backend.Libs.Database.Account;

public interface IAccountRepository : IGenericAsyncUuidRepository<AccountDTO>
{
    Task<AccountDTO?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<AccountDTO?> GetAccountByIpAsync(string ip, CancellationToken cancellationToken);
    Task<AccountDTO?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}