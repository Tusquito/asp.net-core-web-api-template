using System.Threading.Tasks;
using Backend.Api.Services.Account;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Models.Login;
using Backend.Libs.Security.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers.AnonymousControllers;

[ApiController]
[Route("/login")]
public class LoginController : GenericController
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    
    public LoginController(IUserAuthenticationService userAuthenticationService)
    {
        _userAuthenticationService = userAuthenticationService;
    }
    
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginForm form)
    {
        if (string.IsNullOrWhiteSpace(form.Login))
        {
            return BadRequestResponse(ResponseMessageKey.LOGIN_LOGIN_CANT_BE_NULL);
        }

        if (string.IsNullOrWhiteSpace(form.Password))
        {
            return BadRequestResponse(ResponseMessageKey.LOGIN_PASSWORD_CANT_BE_NULL);
        }

        (AccountDTO accountDto, AuthenticationResultType resultType) = await _userAuthenticationService.AuthenticateAsync(form);
        
        switch (resultType)
        {
            case AuthenticationResultType.INVALID_LOGIN:
                return BadRequestResponse(ResponseMessageKey.LOGIN_WRONG_LOGIN);
            case AuthenticationResultType.INVALID_PASSWORD:
                return BadRequestResponse(ResponseMessageKey.LOGIN_WRONG_PASSWORD);
            default:
            case AuthenticationResultType.SUCCESS:
                return OkResponse(new LoginResponseModel
                {
                    AccessToken = accountDto.GenerateJwtToken()
                });
        }
    }
}