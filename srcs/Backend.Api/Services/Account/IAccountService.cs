using System.Threading.Tasks;
using Backend.Api.Models.Login;
using Backend.Domain.Account;
using Backend.Domain.Enums;

namespace Backend.Api.Services.Account;

public interface IAccountService
{
    Task<(AccountDto, AuthenticationResultType)> AuthenticateAsync(LoginForm form);
}