using System.Threading.Tasks;
using Backend.Api.Database.Generic;
using Backend.Domain.Account;

namespace Backend.Api.Database.Account;

public interface IAccountDao : IGenericAsyncUuidRepository<AccountDto>
{
    Task<AccountDto> GetAccountByUsernameAsync(string username);
    Task<AccountDto> GetAccountByIpAsync(string ip);
    Task<AccountDto> GetAccountByEmailAsync(string email);
}