using Backend.Libs.Cryptography.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[Route("/account")]
[ApiController]
public class AccountController : GenericController
{
    //private readonly IAccountDAO _accountDao;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordHasherService _passwordHasherService;

    public AccountController( IHttpContextAccessor httpContextAccessor, IPasswordHasherService passwordHasherService)
    {

        _httpContextAccessor = httpContextAccessor;
        _passwordHasherService = passwordHasherService;
    }

    // GET api/accounts
    // GET api/accounts?Id=ddddd
    /*[HttpGet]
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

        return OkResponse(await _accountDao.GetAllAsync());
    }*/
}