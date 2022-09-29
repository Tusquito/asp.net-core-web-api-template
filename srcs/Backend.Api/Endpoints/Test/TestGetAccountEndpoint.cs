using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Account.Request;
using Backend.Libs.Security.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Endpoints.Test;

[AuthorityRequired(AuthorityType.Admin)]
public class TestGetAccountEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly IAccountService _accountService;

    public TestGetAccountEndpoint(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpGet("api/tests/account")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        var response = await _accountService.GetAccountByIdAsync(new GrpcGetAccountByIdRequest
        {
            Id = Guid.Parse("59001090-b7f7-47aa-911b-cbccbdf6857c")
        }, cancellationToken: cancellationToken);
        return EndpointResult.Ok(response.AccountDto);
    }
}