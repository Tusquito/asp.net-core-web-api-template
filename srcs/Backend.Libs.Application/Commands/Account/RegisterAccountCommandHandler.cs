using Backend.Libs.Application.Commands.Abstractions;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Services.Cryptography.Abstractions;
using Backend.Libs.Infrastructure.Services.Account.Abstractions;
using Backend.Libs.Persistence.Data.Account;
using Backend.Libs.Persistence.Enums;

namespace Backend.Libs.Application.Commands.Account;

public class RegisterAccountCommandHandler : ICommandHandler<RegisterAccountCommand>
{
    private readonly IAccountService _accountService;
    private readonly IPasswordHasherService _hasherService;

    public RegisterAccountCommandHandler(IAccountService accountService, IPasswordHasherService hasherService)
    {
        _accountService = accountService;
        _hasherService = hasherService;
    }

    public async Task<Result> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        Result<AccountDto> result = await _accountService.GetByUsernameAsync(request.Username, cancellationToken);

        if (result.Successful)
        {
            return Result.BadRequest(ResultMessageKey.BadRequestUnavailableUsername);
        }

        result = await _accountService.GetByEmailAsync(request.Email, cancellationToken);

        if (result.Successful)
        {
            return Result.BadRequest(ResultMessageKey.BadRequestUnavailableEmail);
        }

        string passwordSalt = _hasherService.GenerateRandomSalt();
        
        AccountDto accountDto = new AccountDto
        {
            Email = request.Email,
            Password = _hasherService.HashPassword(request.Password, passwordSalt),
            Username = request.Username,
            PasswordSalt = passwordSalt,
            Roles = new List<RoleType> { RoleType.User },
            Ip = request.Ip
        };

        return await _accountService.AddAsync(accountDto, cancellationToken);
    }
}