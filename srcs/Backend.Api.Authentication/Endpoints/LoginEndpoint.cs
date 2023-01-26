using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Commands.Authentication;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Models.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Authentication.Endpoints;

public class LoginEndpoint : EndpointBaseAsync
    .WithRequest<LoginRequest>
    .WithoutResult
{

    private readonly ISender _sender;
    public LoginEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("/login")]
    public override async Task<IActionResult> HandleAsync([FromBody] LoginRequest request, CancellationToken cancellationToken = new())
    {
        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return DomainResults.BadRequest(ResultMessageKey.BadRequestNullLogin);
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return DomainResults.BadRequest(ResultMessageKey.BadRequestNullPassword);
        }

        AuthenticateAccountCommand command = new AuthenticateAccountCommand(request.Login, request.Password);

        Result<TokenModel> result = await _sender.Send(command, cancellationToken);

        return result.ToActionResult();
    }
}