using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Abstractions;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Services.Account;
using Backend.Libs.gRPC.Account.Request;
using Backend.Libs.gRPC.Enums;

namespace Backend.Libs.Domain.Commands.Authentication;

public class RegisterAccountCommandHandler : ICommandHandler<RegisterAccountCommand>
{
    private readonly IAccountService _accountService;

    public RegisterAccountCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        Result<AccountDto?> dto = await _accountService.GetByUsernameAsync(request.Username, cancellationToken);

        if (dto.Type != ResultType.NotFound)
        {
            
        }
        
        if (accountResponse.Type == GrpcResponseType.Success)
        {
            return DomainResults.BadRequest(ResultMessageKey.BadRequestUnavailableUsername, requestForm.Username);
        }

        accountResponse = await _grpcAccountService.GetAccountByEmailAsync(new GrpcGetAccountByStringRequest
        {
            Search = requestForm.Email
        }, cancellationToken: cancellationToken);

        if (accountResponse.Type == GrpcResponseType.Success)
        {
            return DomainResults.BadRequest(ResultMessageKey.BadRequestUnavailableEmail, requestForm.Email);
        }

        try
        {
            AccountDto accountDto = requestForm.Adapt<AccountDto>();
            accountDto.PasswordSalt = _passwordHasherService.GenerateRandomSalt();
            accountDto.Password = _passwordHasherService.HashPassword(requestForm.Password, accountDto.PasswordSalt);
            accountDto.Ip = requesterIp;
            accountDto.Roles = new List<RoleType> { RoleType.User };
            await _grpcAccountService.UpdateAccountAsync(new GrpcSaveAccountRequest
            {
                AccountDto = accountDto
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(RegisterEndpoint));
            return DomainResults.InternalServerError(ResultMessageKey.InternalServerErrorEntitySaveError,
                nameof(AccountDto));
        }

        return DomainResults.Ok();
    }
}