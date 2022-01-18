using System;
using System.Threading.Tasks;
using Backend.Api.Database.Account;
using Backend.Api.Models.Account;
using Backend.Domain.Account;
using Backend.Domain.Enums;
using Backend.Libs.Caching.Repositories;
using Backend.Libs.Security.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[Route("/account")]
[ApiController]
public class AccountController : GenericController
{
    private readonly IAccountDao _accountDao;
    private readonly GuidMemoryCacheRepository<AccountDto> _memoryCache;

    public AccountController(IAccountDao accountDao, GuidMemoryCacheRepository<AccountDto> memoryCache)
    {
        _accountDao = accountDao;
        _memoryCache = memoryCache;
    }

    // GET api/accounts
    // GET api/accounts?Id=ddddd
    [HttpGet]
    [AuthorityRequired(AuthorityType.USER)]
    public async Task<IActionResult> GetAccountsByQueryAsync([FromQuery] AccountRequestQuery query)
    {
        if (query.Id != Guid.Empty)
        {
            return OkResponse(new[] {_memoryCache.Get(query.Id) ?? await _memoryCache.SetOrCreateAsync(query.Id, await _accountDao.GetByIdAsync(query.Id))});
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

        return OkResponse(await _accountDao.GetAllAsync());
    }
}