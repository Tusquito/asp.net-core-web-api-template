using System.Threading.Tasks;
using Backend.Api.Database.Account;
using Backend.Api.Models.Login;
using Backend.Domain.Account;
using Backend.Domain.Enums;
using Backend.Libs.Cryptography.Services;

namespace Backend.Api.Services.Account;

public class AccountService : IAccountService
{
    private readonly IAccountDao _accountDao;
    private readonly IPasswordHasherService _passwordHasherService;

    public AccountService(IAccountDao accountDao, IPasswordHasherService passwordHasherService)
    {
        _accountDao = accountDao;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<(AccountDto, AuthenticationResultType)> AuthenticateAsync(LoginForm form)
    {
        AccountDto accountByEmail = await _accountDao.GetAccountByEmailAsync(form.Login);
        AccountDto accountByUsername = await _accountDao.GetAccountByUsernameAsync(form.Login);
        
        if (accountByEmail == null && accountByUsername == null)
        {
            return (null, AuthenticationResultType.INVALID_LOGIN);
        }

        AccountDto currentAccount = accountByEmail ?? accountByUsername;
        return currentAccount.Password != _passwordHasherService.HashPassword(form.Password, currentAccount.PasswordSalt)
            ? (null, AuthenticationResultType.INVALID_PASSWORD) 
            : (currentAccount, AuthenticationResultType.SUCCESS);
    }
}