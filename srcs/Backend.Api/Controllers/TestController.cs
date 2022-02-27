using System;
using System.Threading.Tasks;
using Backend.Libs.Cryptography.Services;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Grpc.Account;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[Route("api/tests")]
[ApiController]
public class TestController : GenericController
{
    private readonly GrpcAccountService.GrpcAccountServiceClient _accountService;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IHttpContextAccessor _contextAccessor;

    public TestController(GrpcAccountService.GrpcAccountServiceClient accountService, IPasswordHasherService passwordHasherService, IHttpContextAccessor contextAccessor)
    {
        _accountService = accountService;
        _passwordHasherService = passwordHasherService;
        _contextAccessor = contextAccessor;
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetDefaultUser()
    {
        var response = await _accountService.GetAccountByIdAsync(new GetAccountByIdRequest { Id = Guid.Parse("59001090-b7f7-47aa-911b-cbccbdf6857c") });

        Guid test = response.GrpcAccountDto.Id;
        return Ok(response.GrpcAccountDto.Adapt<AccountDTO>());
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddDefaultUser()
    {
        string salt = _passwordHasherService.GenerateRandomSalt();
        var response = await _accountService.AddAccountAsync(new AddAccountRequest
        {
            GrpcAccountDto = new GrpcAccountDTO
            {
                Id = Guid.Empty,
                AuthorityType = GrpcAuthorityType.Admin,
                Username = "admin",
                Password = _passwordHasherService.HashPassword("test", salt),
                Email = "admin@gmail.com",
                Ip = _contextAccessor.GetRequestIp(),
                PasswordSalt = salt
            }
        });
        return Ok(response);
    }

    // POST
    /*[HttpPost("database")]
    public async Task<IActionResult> CreateTestDatabaseAsync()
    {
        await _backendDbContext.Database.EnsureCreatedAsync();
        var accountEntities = new List<AccountEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Email = Guid.NewGuid().ToString(),
                Ip = new IPAddress(_random.Next()).ToString(),
                Username = "admin",
                Password = "admin",
                TestId = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Email = Guid.NewGuid().ToString(),
                Ip = new IPAddress(_random.Next()).ToString(),
                Username = "test",
                Password = "admin",
                TestId = Guid.NewGuid()
            }
        };
        await _backendDbContext.AddRangeAsync(accountEntities);
        await _backendDbContext.SaveChangesAsync();
        return OkResponse();
    }*/
}