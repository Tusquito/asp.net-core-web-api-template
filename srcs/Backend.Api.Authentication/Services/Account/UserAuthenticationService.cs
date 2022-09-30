using System.Threading;
using System.Threading.Tasks;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Models.Login;

namespace Backend.Api.Authentication.Services.Account;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasherService _passwordHasherService;

    public UserAuthenticationService(IAccountRepository accountRepository, IPasswordHasherService passwordHasherService)
    {
        _accountRepository = accountRepository;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<(AccountDTO, AuthenticationResultType)> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        AccountDTO accountByEmail = await _accountRepository.GetByEmailAsync(request.Login, cancellationToken);
        AccountDTO accountByUsername = await _accountRepository.GetByUsernameAsync(request.Login, cancellationToken);
        
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