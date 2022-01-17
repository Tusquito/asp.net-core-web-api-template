using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Api.Database.Context;
using Backend.Api.Database.Generic;
using Backend.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Api.Database.Account;

public class AccountDao : IAccountDao
{
    private readonly IGenericMapper<AccountEntity, AccountDto> _mapper;
    private readonly IDbContextFactory<BackendDbContext> _contextFactory;
    private readonly IGenericAsyncUuidRepository<AccountDto> _repository;
    private readonly ILogger<AccountDao> _logger;

    public AccountDao(IGenericMapper<AccountEntity, AccountDto> mapper, IDbContextFactory<BackendDbContext> contextFactory, IGenericAsyncUuidRepository<AccountDto> repository, ILogger<AccountDao> logger)
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
        _repository = repository;
        _logger = logger;
    }
        
    public async Task<AccountDto> GetAccountByUsernameAsync(string username)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync();
            return _mapper.Map(await context.Accounts.FirstOrDefaultAsync(x => x.Username.Equals(username.ToLowerInvariant())));
        }
        catch (Exception e)
        {
            _logger.LogError($"{nameof(GetAccountByUsernameAsync)} {e}");
            return null;
        }
    }

    public async Task<AccountDto> GetAccountByIpAsync(string ip)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync();
            return _mapper.Map(await context.Accounts.FirstOrDefaultAsync(x => x.Ip.Equals(ip)));
        }
        catch (Exception e)
        {
            _logger.LogError($"{nameof(GetAccountByIpAsync)} {e}");
            return null;
        }
    }

    public async Task<AccountDto> GetAccountByEmailAsync(string email)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync();
            return _mapper.Map(await context.Accounts.FirstOrDefaultAsync(x => x.Email.Equals(email.ToLower())));
        }
        catch (Exception e)
        {
            _logger.LogError($"{nameof(GetAccountByEmailAsync)} {e}");
            return null;
        }
    }

    public async Task<IEnumerable<AccountDto>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<AccountDto> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("GetByIdAsync called");
        return await _repository.GetByIdAsync(id);
    }
    public async Task<IEnumerable<AccountDto>> GetByIdsAsync(IEnumerable<Guid> ids) => await _repository.GetByIdsAsync(ids);
    public async Task<AccountDto> SaveAsync(AccountDto obj)
    {
        obj.Username = obj.Username.ToLowerInvariant();
        obj.Email = obj.Email.ToLowerInvariant();
        return await _repository.SaveAsync(obj);
    } 
    public async Task<IEnumerable<AccountDto>> SaveAsync(IReadOnlyList<AccountDto> objs)
    {
        foreach (var accountDto in objs)
        {
            accountDto.Username = accountDto.Username.ToLowerInvariant();
            accountDto.Email = accountDto.Email.ToLowerInvariant();
        }

        return await _repository.SaveAsync(objs);
    }
    public async Task DeleteByIdAsync(Guid id) => await _repository.DeleteByIdAsync(id);
    public async Task DeleteByIdsAsync(IEnumerable<Guid> ids) => await _repository.DeleteByIdsAsync(ids);
}