using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AuthPermissions.AspNetCore;
using Backend.Libs.Domain.Enums;
using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Account.Request;
using Backend.Plugins.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Endpoints.Test;

public class TestGetAccountEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly IGrpcAccountService _grpcAccountService;

    public TestGetAccountEndpoint(IGrpcAccountService grpcAccountService)
    {
        _grpcAccountService = grpcAccountService;
    }
    
    [HttpGet("api/tests/account")]
    [HasPermission(PermissionsType.TestGetAccount)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        var response = await _grpcAccountService.GetAccountByIdAsync(new GrpcGetAccountByIdRequest
        {
            Id = Guid.Parse("59001090-b7f7-47aa-911b-cbccbdf6857c")
        }, cancellationToken: cancellationToken);
        return DomainResults.Ok(response.AccountDto);
    }
}