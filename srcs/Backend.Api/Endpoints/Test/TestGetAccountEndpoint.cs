using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Attributes;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Domain.Services.Account;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Endpoints.Test;

public class TestGetAccountEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithoutResult
{
    private readonly IAccountService _accountService;

    public TestGetAccountEndpoint(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpGet("api/tests/account")]
    [HasPermission(PermissionType.TestGetAccount)]
    public override async Task<IActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        Result<AccountDto?> res =
            await _accountService.GetByIdAsync(Guid.Parse("59001090-b7f7-47aa-911b-cbccbdf6857c"), cancellationToken);

        return res.ToActionResult();
    }
}