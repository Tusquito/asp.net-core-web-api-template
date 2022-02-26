using System;
using System.Threading.Tasks;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Models.Account;
using EmailValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers.AnonymousControllers;

[ApiController]
[Route("/register")]
public class RegisterController : GenericController
{
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountDAO _accountDao;

    public RegisterController(IPasswordHasherService passwordHasherService, IHttpContextAccessor httpContextAccessor, IAccountDAO accountDao)
    {
        _passwordHasherService = passwordHasherService;
        _httpContextAccessor = httpContextAccessor;
        _accountDao = accountDao;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAccountAsync([FromBody] AccountRegisterForm form)
    {
        string requesterIp = _httpContextAccessor.GetRequestIp();
        if (form.Password != form.PasswordConfirmation)
        {
            return BadRequestResponse(ResponseMessageKey.REGISTER_INVALID_PASSWORD_CONFIRMATION);
        }

        if (!EmailValidator.Validate(form.Email))
        {
            return BadRequestResponse(ResponseMessageKey.REGISTER_INVALID_EMAIL_FORMAT);
        }

        AccountDTO currentAccount = await _accountDao.GetAccountByUsernameAsync(form.Username);

        if (currentAccount != null)
        {
            return BadRequestResponse(ResponseMessageKey.REGISTER_USERNAME_ALREADY_TAKEN, new []{form.Username});
        }

        currentAccount = await _accountDao.GetAccountByEmailAsync(form.Email);

        if (currentAccount != null)
        {
            return BadRequestResponse(ResponseMessageKey.REGISTER_EMAIL_ALREADY_TAKEN, new []{form.Email});
        }

        try
        {
            currentAccount = form.Adapt<AccountDTO>();
            currentAccount.PasswordSalt = _passwordHasherService.GenerateRandomSalt();
            currentAccount.Password =
                _passwordHasherService.HashPassword(form.Password, currentAccount.PasswordSalt);
            currentAccount.Ip = requesterIp;
            currentAccount.AuthorityType = AuthorityType.User;
            await _accountDao.UpdateAsync(currentAccount);
        }
        catch (Exception e)
        {
            return InternalServerErrorResponse(ResponseMessageKey.REGISTER_ACCOUNT_SAVING_FAILED,
                new[] { e.Message });
        }
            
        return OkResponse(messageKey:ResponseMessageKey.REGISTER_SUCCESS);
    }
}