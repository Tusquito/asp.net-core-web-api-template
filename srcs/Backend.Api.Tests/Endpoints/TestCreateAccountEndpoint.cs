using Ardalis.ApiEndpoints;
using Backend.Api.Tests.Commands;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Tests.Endpoints;

public class TestCreateAccountEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithoutResult
{

    private readonly ISender _sender;
    public TestCreateAccountEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("api/tests/account")]
    [ProducesResponseType(typeof(GenericResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [Produces("application/json")]
    public override async Task<IActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        CreateAccountCommand command = new CreateAccountCommand("tester", "tester", "tester@.gmail.com", new List<RoleType>
        {
            RoleType.Tester
        }, Guid.Parse("59001090-b7f7-47aa-911b-cbccbdf6857c"));
        
        Result result = await _sender.Send(command, cancellationToken);

        return result.ToActionResult();
    }
}