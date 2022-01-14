using System;
using System.Threading.Tasks;
using Backend.Api.Database.Account;
using Backend.Api.Extensions;
using Backend.Api.Models.Account;
using Backend.Domain.Account;
using Backend.Domain.Enums;
using Backend.Libs.Cryptography.Services;
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
    private readonly IAccountDao _accountDao;

    public RegisterController(IPasswordHasherService passwordHasherService, IHttpContextAccessor httpContextAccessor, IAccountDao accountDao)
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

        AccountDto currentAccount = await _accountDao.GetAccountByUsernameAsync(form.Username);

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
            currentAccount = form.Adapt<AccountDto>();
            currentAccount.PasswordSalt = _passwordHasherService.GenerateRandomSalt();
            currentAccount.Password =
                _passwordHasherService.HashPassword(form.Password, currentAccount.PasswordSalt);
            currentAccount.Ip = requesterIp;
            currentAccount.AuthorityType = AuthorityType.USER;
            await _accountDao.SaveAsync(currentAccount);
        }
        catch (Exception e)
        {
            return InternalServerErrorResponse(ResponseMessageKey.REGISTER_ACCOUNT_SAVING_FAILED,
                new[] { e.Message });
        }
            
        return OkResponse(messageKey:ResponseMessageKey.REGISTER_SUCCESS);
    }
}