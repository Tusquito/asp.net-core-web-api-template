using Ardalis.ApiEndpoints;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Attributes;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Infrastructure.Services.Account.Abstractions;
using Backend.Libs.Persistence.Data.Account;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Tests.Endpoints;

public class TestGetAccountEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithoutResult
{
    private readonly IAccountService _accountService;

    public TestGetAccountEndpoint(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpGet("account")]
    [HasPermission(PermissionType.TestGetAccount)]
    public override async Task<IActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        Result<AccountDto?> res =
            await _accountService.GetByIdAsync(Guid.Parse("59001090-b7f7-47aa-911b-cbccbdf6857c"), cancellationToken);

        return res.ToActionResult();
    }
}