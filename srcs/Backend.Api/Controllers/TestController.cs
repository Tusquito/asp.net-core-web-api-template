using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Backend.Api.Database.Account;
using Backend.Api.Database.Context;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestController : GenericController
    {
        private readonly BackendDbContext _backendDbContext;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());

        public TestController(BackendDbContext backendDbContext)
        {
            _backendDbContext = backendDbContext;
        }
        
        // POST
        [HttpPost("database")]
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

            var testEntities = accountEntities.Select(x => new TestEntity { Id = x.TestId }).ToList();
            await _backendDbContext.AddRangeAsync(testEntities);
            await _backendDbContext.AddRangeAsync(accountEntities);
            await _backendDbContext.SaveChangesAsync();
            return OkResponse();
        }
    }
}