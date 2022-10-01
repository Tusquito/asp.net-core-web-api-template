using System;
using Backend.Libs.Database.Account;

namespace Backend.Libs.Domain.Services.Account;

public interface IAccountService : IRedisService<AccountDTO, Guid>
{
    
}