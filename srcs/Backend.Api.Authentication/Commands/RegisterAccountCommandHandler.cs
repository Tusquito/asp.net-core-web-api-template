using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Services.Account;
using Backend.Libs.Mediator.Commands.Abstractions;

namespace Backend.Api.Authentication.Commands;

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
        Result<AccountDto?> result = await _accountService.GetByUsernameAsync(request.Username, cancellationToken);

        if (result.Successful)
        {
            return Result.BadRequest(ResultMessageKey.BadRequestUnavailableUsername);
        }

        result = await _accountService.GetByEmailAsync(request.Email, cancellationToken);

        if (result.Successful)
        {
            return Result.BadRequest(ResultMessageKey.BadRequestUnavailableEmail);
        }

        AccountDto accountDto = new AccountDto
        {
            Email = request.Email,
            Password = request.Password,
            Username = request.Username,
            PasswordSalt = _hasherService.GenerateRandomSalt(),
            Roles = new List<RoleType> { RoleType.User },
            Ip = request.Ip,

        };

        accountDto.Password = _hasherService.HashPassword(request.Password, accountDto.PasswordSalt);

        return await _accountService.AddAsync(accountDto, cancellationToken);
    }
}