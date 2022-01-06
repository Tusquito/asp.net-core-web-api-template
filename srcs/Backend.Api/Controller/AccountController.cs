using System;
using System.Threading.Tasks;
using Backend.Api.Database.Account;
using Backend.Api.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controller
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : GenericController
    {
        private readonly IAccountDao _accountDao;

        public AccountController(IAccountDao accountDao)
        {
            _accountDao = accountDao;
        }

        // GET api/accounts
        // GET api/accounts?Id=ddddd
        [HttpGet]
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
        }
    }
}