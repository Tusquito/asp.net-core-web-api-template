using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Api.Authentication.Services.Account;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Models.Login;
using Backend.Libs.Security.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Authentication.Endpoints;

public class LoginEndpoint : EndpointBaseAsync
    .WithRequest<LoginRequest>
    .WithActionResult
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public LoginEndpoint(IUserAuthenticationService userAuthenticationService)
    {
        _userAuthenticationService = userAuthenticationService;
    }

    [HttpPost("/login")]
    public override async Task<ActionResult> HandleAsync([FromBody] LoginRequest request, CancellationToken cancellationToken = new())
    {
        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return EndpointResult.BadRequest(ResultMessageKey.BAD_REQUEST_NULL_LOGIN);
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return EndpointResult.BadRequest(ResultMessageKey.BAD_REQUEST_NULL_PASSWORD);
        }

        (AccountDTO accountDto, AuthenticationResultType resultType) = await _userAuthenticationService.AuthenticateAsync(request);
    
        switch (resultType)
        {
            case AuthenticationResultType.INVALID_LOGIN:
                return EndpointResult.BadRequest(ResultMessageKey.BAD_REQUEST_INVALID_LOGIN);
            case AuthenticationResultType.INVALID_PASSWORD:
                return EndpointResult.BadRequest(ResultMessageKey.BAD_REQUEST_INVALID_PASSWORD);
            default:
            case AuthenticationResultType.SUCCESS:
                return EndpointResult.Ok(new LoginResponseModel
                {
                    AccessToken = accountDto.GenerateJwtToken()
                });
        }
    }
}