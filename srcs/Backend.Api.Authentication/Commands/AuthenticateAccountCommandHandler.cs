using System.Threading;
using System.Threading.Tasks;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Services.Account;
using Backend.Libs.Mediator.Commands.Abstractions;
using Backend.Libs.Models.Authentication;
using Backend.Libs.Security.Extensions;

namespace Backend.Api.Authentication.Commands;

public class AuthenticateAccountCommandHandler : ICommandHandler<AuthenticateAccountCommand, TokenModel>
{
    private readonly IAccountService _accountService;
    private readonly IPasswordHasherService _passwordHasherService;

    public AuthenticateAccountCommandHandler(IAccountService accountService, IPasswordHasherService passwordHasherService)
    {
        _accountService = accountService;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<Result<TokenModel>> Handle(AuthenticateAccountCommand request, CancellationToken cancellationToken)
    {
        Result<AccountDto?> result = await _accountService.GetByEmailAsync(request.Login, cancellationToken);
        
        if (result.Type == ResultType.NotFound || result.Value == null)
        {
            result = await _accountService.GetByUsernameAsync(request.Login, cancellationToken);
        }

        if (result.Type != ResultType.Success && !ResultType.Success.HasFlag(result.Type) || result.Value == null)
        {
            return new Result<TokenModel>(result.Type, result.Message);
        }
        
        return result.Value.Password != _passwordHasherService.HashPassword(request.Password, result.Value.PasswordSalt)
            ? Result<TokenModel>.BadRequest(ResultMessageKey.BadRequestWrongPassword)
            : Result<TokenModel>.Ok(new TokenModel { AccessToken = result.Value.GenerateJwtToken() });
    }
}