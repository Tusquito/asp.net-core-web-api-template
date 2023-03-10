using Backend.Libs.Application.Commands.Abstractions;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Domain.Services.Cryptography.Abstractions;
using Backend.Libs.Infrastructure.Services.Account.Abstractions;
using Backend.Libs.Persistence.Data.Account;
using Microsoft.AspNetCore.Http;

namespace Backend.Libs.Application.Commands.Account;

public class CreateAccountHandler : ICommandHandler<CreateAccountCommand>
{
    private readonly IAccountService _accountService;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateAccountHandler(IAccountService accountService, IPasswordHasherService passwordHasherService, IHttpContextAccessor httpContextAccessor)
    {
        _accountService = accountService;
        _passwordHasherService = passwordHasherService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        string salt = _passwordHasherService.GenerateRandomSalt();

        return await _accountService.AddAsync(new AccountDto
        {
            Id = request.Id != default ? request.Id : Guid.NewGuid(),
            Roles = request.Roles,
            Username = request.Username,
            Password = _passwordHasherService.HashPassword(request.Password, salt),
            Email = request.Email,
            Ip = _httpContextAccessor.GetRequestIpOrThrow(),
            PasswordSalt = salt
        }, cancellationToken);
    }
}