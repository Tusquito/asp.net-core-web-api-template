using System.Threading.Tasks;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Models.Login;

namespace Backend.Api.Authentication.Services.Account;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IAccountDAO _accountDao;
    private readonly IPasswordHasherService _passwordHasherService;

    public UserAuthenticationService(IAccountDAO accountDao, IPasswordHasherService passwordHasherService)
    {
        _accountDao = accountDao;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<(AccountDTO, AuthenticationResultType)> AuthenticateAsync(LoginRequest request)
    {
        AccountDTO accountByEmail = await _accountDao.GetByEmailAsync(request.Login);
        AccountDTO accountByUsername = await _accountDao.GetByUsernameAsync(request.Login);
        
        if (accountByEmail == null && accountByUsername == null)
        {
            return (null, AuthenticationResultType.INVALID_LOGIN);
        }

        AccountDTO currentAccount = accountByEmail ?? accountByUsername;
        return currentAccount.Password != _passwordHasherService.HashPassword(request.Password, currentAccount.PasswordSalt)
            ? (null, AuthenticationResultType.INVALID_PASSWORD) 
            : (currentAccount, AuthenticationResultType.SUCCESS);
    }
}