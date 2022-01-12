using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Api.Database.Account;
using Backend.Api.Extensions;
using Backend.Api.Models.Account;
using Backend.Domain.Account;
using Backend.Domain.Enums;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Security.Attributes;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[Route("/account")]
[ApiController]
public class AccountController : GenericController
{
    private readonly IAccountDao _accountDao;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordHasherService _passwordHasherService;

    public AccountController(IAccountDao accountDao, IHttpContextAccessor httpContextAccessor, IPasswordHasherService passwordHasherService)
    {
        _accountDao = accountDao;
        _httpContextAccessor = httpContextAccessor;
        _passwordHasherService = passwordHasherService;
    }

    // GET api/accounts
    // GET api/accounts?Id=ddddd
    [HttpGet]
    [AuthorityRequired(AuthorityType.USER)]
    public async Task<IActionResult> GetAccountsByQueryAsync([FromQuery] AccountRequestQuery query)
    {
        if (query.Id != Guid.Empty)
        {
            return OkResponse(new[] {await _accountDao.GetByIdAsync(query.Id)});
        }

        if (!string.IsNullOrEmpty(query.Username))
        {
            return OkResponse(new[] {await _accountDao.GetAccountByUsernameAsync(query.Username)});
        }

        if (!string.IsNullOrEmpty(query.Ip))
        {
            return OkResponse(new[] {await _accountDao.GetAccountByIpAsync(query.Ip)});
        }

        if (!string.IsNullOrEmpty(query.Email))
        {
            return OkResponse(new[] {await _accountDao.GetAccountByEmailAsync(query.Email)});
        }

        return OkResponse((await _accountDao.GetAllAsync()).Adapt<List<AccountModel>>());
    }
        
        
    [HttpPost]
    public async Task<IActionResult> RegisterAccountAsync([FromBody] AccountRegisterForm form)
    {
        string requesterIp = _httpContextAccessor.GetRequestIp();
        if (form.Password != form.PasswordConfirmation)
        {
            return BadRequestResponse(ResponseMessageKey.REGISTER_INVALID_PASSWORD_CONFIRMATION);
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