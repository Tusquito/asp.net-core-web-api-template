using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.gRPC.Account;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Endpoints.Test;

public class TestGetAccountEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly GrpcAccountService.GrpcAccountServiceClient _accountService;

    public TestGetAccountEndpoint(GrpcAccountService.GrpcAccountServiceClient accountService)
    {
        _accountService = accountService;
    }
    
    [HttpGet("api/tests/account")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        var response = await _accountService.GetAccountByIdAsync(new GetAccountByIdRequest
        {
            Id = Guid.Parse("59001090-b7f7-47aa-911b-cbccbdf6857c")
        }, cancellationToken: cancellationToken);
        return GenericResponses.Ok((AccountDTO)response.GrpcAccountDto);
    }
}