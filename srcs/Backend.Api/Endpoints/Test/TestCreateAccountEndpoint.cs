using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Grpc.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Endpoints.Test;

public class TestCreateAccountEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly GrpcAccountService.GrpcAccountServiceClient _accountService;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TestCreateAccountEndpoint(GrpcAccountService.GrpcAccountServiceClient accountService, IPasswordHasherService passwordHasherService, IHttpContextAccessor httpContextAccessor)
    {
        _accountService = accountService;
        _passwordHasherService = passwordHasherService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpPost("api/tests/account")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        string salt = _passwordHasherService.GenerateRandomSalt();
        var response = await _accountService.AddAccountAsync(new AddAccountRequest
        {
            GrpcAccountDto = new GrpcAccountDTO
            {
                Id = Guid.NewGuid(),
                AuthorityType = GrpcAuthorityType.Admin,
                Username = "admin",
                Password = _passwordHasherService.HashPassword("test", salt),
                Email = "admin@gmail.com",
                Ip = _httpContextAccessor.GetRequestIp(),
                PasswordSalt = salt
            }
        });

        return GenericResponses.Ok(response);
    }
}