using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Backend.Api.Database.Account;
using Backend.Api.Database.Context;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controller
{
    [Route("api/tests")]
    [ApiController]
    public class TestController : GenericController
    {
        private readonly WebContext _webContext;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());

        public TestController(WebContext webContext)
        {
            _webContext = webContext;
        }
        
        // POST
        [HttpPost("database")]
        public async Task<IActionResult> CreateTestDatabaseAsync()
        {
            await _webContext.Database.EnsureCreatedAsync();
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
            await _webContext.AddRangeAsync(testEntities);
            await _webContext.AddRangeAsync(accountEntities);
            await _webContext.SaveChangesAsync();
            return OkResponse();
        }
    }
}