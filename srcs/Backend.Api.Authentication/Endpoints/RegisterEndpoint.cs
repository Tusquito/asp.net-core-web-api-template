using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Account.Request;
using Backend.Libs.gRPC.Account.Responses;
using Backend.Libs.gRPC.Enums;
using Backend.Libs.Models.Account;
using EmailValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Api.Authentication.Endpoints;

public class RegisterEndpoint : EndpointBaseAsync
    .WithRequest<RegisterRequest>
    .WithActionResult
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IAccountService _accountService;
    private readonly ILogger<RegisterEndpoint> _logger;

    public RegisterEndpoint(IHttpContextAccessor httpContextAccessor, IPasswordHasherService passwordHasherService, ILogger<RegisterEndpoint> logger, IAccountService accountService)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordHasherService = passwordHasherService;
        _logger = logger;
        _accountService = accountService;
    }

    [HttpPost("/register")]
    public override async Task<ActionResult> HandleAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken = new())
    {
        string requesterIp = _httpContextAccessor.GetRequestIp();
        if (request.Password != request.PasswordConfirmation)
        {
            return EndpointResult.BadRequest(ResultMessageKey.BAD_REQUEST_DIFFERENT_PASSWORD_CONFIRMATION);
        }

        if (!EmailValidator.Validate(request.Email))
        {
            return EndpointResult.BadRequest(ResultMessageKey.BAD_REQUEST_INVALID_EMAIL_FORMAT);
        }

        GrpcAccountResponse accountResponse = await _accountService.GetAccountByUsernameAsync(new GrpcGetAccountByStringRequest
        {
            Search = request.Username
        }, cancellationToken);

        if (accountResponse.Type == GrpcResponseType.Success)
        {
            return EndpointResult.BadRequest(ResultMessageKey.BAD_REQUEST_UNAVAILABLE_USERNAME, new []{request.Username});
        }

        accountResponse = await _accountService.GetAccountByEmailAsync(new GrpcGetAccountByStringRequest
        {
            Search = request.Email
        }, cancellationToken: cancellationToken);

        if (accountResponse.Type == GrpcResponseType.Success)
        {
            return EndpointResult.BadRequest(ResultMessageKey.BAD_REQUEST_UNAVAILABLE_EMAIL, new []{request.Email});
        }

        try
        {
            AccountDTO accountDto = request.Adapt<AccountDTO>();
            accountDto.PasswordSalt = _passwordHasherService.GenerateRandomSalt();
            accountDto.Password = _passwordHasherService.HashPassword(request.Password, accountDto.PasswordSalt);
            accountDto.Ip = requesterIp;
            accountDto.AuthorityType = AuthorityType.User;
            await _accountService.UpdateAccountAsync(new GrpcSaveAccountRequest
            {
                AccountDto = accountDto
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(RegisterEndpoint));
            return EndpointResult.InternalServerError(ResultMessageKey.INTERNAL_SERVER_ERROR_ENTITY_SAVE_ERROR, new[] { nameof(AccountDTO) });
        }
            
        return EndpointResult.Ok();
    }
}