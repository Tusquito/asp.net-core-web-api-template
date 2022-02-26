using System.Threading.Tasks;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Models.Login;

namespace Backend.Api.Services.Account;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IAccountDAO _accountDao;
    private readonly IPasswordHasherService _passwordHasherService;

    public UserAuthenticationService(IAccountDAO accountDao, IPasswordHasherService passwordHasherService)
    {
        _accountDao = accountDao;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<(AccountDTO, AuthenticationResultType)> AuthenticateAsync(LoginForm form)
    {
        AccountDTO accountByEmail = await _accountDao.GetAccountByEmailAsync(form.Login);
        AccountDTO accountByUsername = await _accountDao.GetAccountByUsernameAsync(form.Login);
        
        if (accountByEmail == null && accountByUsername == null)
        {
            return (null, AuthenticationResultType.INVALID_LOGIN);
        }

        AccountDTO currentAccount = accountByEmail ?? accountByUsername;
        return currentAccount.Password != _passwordHasherService.HashPassword(form.Password, currentAccount.PasswordSalt)
            ? (null, AuthenticationResultType.INVALID_PASSWORD) 
            : (currentAccount, AuthenticationResultType.SUCCESS);
    }
}