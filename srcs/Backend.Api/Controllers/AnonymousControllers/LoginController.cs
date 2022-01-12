using System;
using System.Threading.Tasks;
using Backend.Api.Models.Login;
using Backend.Api.Services.Account;
using Backend.Domain.Account;
using Backend.Domain.Enums;
using Backend.Libs.Security.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers.AnonymousControllers;

[ApiController]
[Route("/login")]
public class LoginController : GenericController
{
    private readonly IAccountService _accountService;
    
    public LoginController(IAccountService accountService)
    {
        _accountService = accountService;
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

        (AccountDto accountDto, AuthenticationResultType resultType) = await _accountService.AuthenticateAsync(form);
        
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