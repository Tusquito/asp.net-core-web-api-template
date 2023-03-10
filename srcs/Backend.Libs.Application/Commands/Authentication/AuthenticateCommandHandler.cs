using Backend.Libs.Application.Commands.Abstractions;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Domain.Models.Authentication;
using Backend.Libs.Domain.Services.Cryptography.Abstractions;
using Backend.Libs.Infrastructure.Services.Account.Abstractions;
using Backend.Libs.Persistence.Data.Account;

namespace Backend.Libs.Application.Commands.Authentication;

public class AuthenticateAccountCommandHandler : ICommandHandler<AuthenticateCommand, TokenModel>
{
    private readonly IAccountService _accountService;
    private readonly IPasswordHasherService _passwordHasherService;

    public AuthenticateAccountCommandHandler(IAccountService accountService, IPasswordHasherService passwordHasherService)
    {
        _accountService = accountService;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<Result<TokenModel>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
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
            : Result<TokenModel>.Ok(new TokenModel
            {
                AccessToken = result.Value.GenerateJwtToken(),
                RefreshToken = string.Empty,
                TokenType = "Bearer"
            });
    }
}