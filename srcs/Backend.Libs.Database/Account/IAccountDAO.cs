using Backend.Libs.Database.Generic;

namespace Backend.Libs.Database.Account;

public interface IAccountDAO : IGenericAsyncUuidRepository<AccountDTO>
{
    Task<AccountDTO?> GetByUsernameAsync(string username);
    Task<AccountDTO?> GetAccountByIpAsync(string ip);
    Task<AccountDTO?> GetByEmailAsync(string email);
}