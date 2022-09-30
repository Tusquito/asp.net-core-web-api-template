using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Account.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Endpoints.Test;

public class TestCreateAccountEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithoutResult
{
    private readonly IGrpcAccountService _grpcAccountService;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TestCreateAccountEndpoint(IGrpcAccountService grpcAccountService, IPasswordHasherService passwordHasherService, IHttpContextAccessor httpContextAccessor)
    {
        _grpcAccountService = grpcAccountService;
        _passwordHasherService = passwordHasherService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpPost("api/tests/account")]
    public override async Task<IActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        string salt = _passwordHasherService.GenerateRandomSalt();
        var response = await _grpcAccountService.AddAccountAsync(new GrpcSaveAccountRequest
        {
            AccountDto = new AccountDTO
            {
                Id = Guid.NewGuid(),
                AuthorityType = AuthorityType.Admin,
                Username = "admin",
                Password = _passwordHasherService.HashPassword("test", salt),
                Email = "admin@gmail.com",
                Ip = _httpContextAccessor.GetRequestIp(),
                PasswordSalt = salt
            }
        }, cancellationToken);

        return DomainResults.Ok(response);
    }
}