using Backend.Libs.Database.Account;
using Backend.Libs.Database.Generic;
using Backend.Plugins.Database.Context;
using Backend.Plugins.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Plugins.Database.DAOs;

public class AccountDAO : IAccountDAO
{
    private readonly IGenericMapper<AccountEntity, AccountDTO?> _mapper;
    private readonly IDbContextFactory<BackendDbContext> _contextFactory;
    private readonly IGenericAsyncUuidRepository<AccountDTO> _repository;
    private readonly ILogger<AccountDAO> _logger;

    public AccountDAO(IGenericMapper<AccountEntity, AccountDTO?> mapper, IDbContextFactory<BackendDbContext> contextFactory, IGenericAsyncUuidRepository<AccountDTO> repository, ILogger<AccountDAO> logger)
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
        _repository = repository;
        _logger = logger;
    }
        
    public async Task<AccountDTO?> GetAccountByUsernameAsync(string username)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync();
            return _mapper.Map(await context.Accounts.FirstOrDefaultAsync(x => x.Username.Equals(username.ToLowerInvariant())));
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"[{nameof(GetAccountByUsernameAsync)}]");
            return null;
        }
    }

    public async Task<AccountDTO?> GetAccountByIpAsync(string ip)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync();
            return _mapper.Map(await context.Accounts.FirstOrDefaultAsync(x => x.Ip.Equals(ip)));
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"[{nameof(GetAccountByIpAsync)}]");
            return null;
        }
    }

    public async Task<AccountDTO?> GetAccountByEmailAsync(string email)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync();
            return _mapper.Map(await context.Accounts.FirstOrDefaultAsync(x => x.Email.Equals(email.ToLower())));
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"[{nameof(GetAccountByEmailAsync)}]");
            return null;
        }
    }

    public async Task<List<AccountDTO>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<AccountDTO?> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);
    public async Task<List<AccountDTO>> GetByIdsAsync(IEnumerable<Guid> ids) => await _repository.GetByIdsAsync(ids);
    public async Task<AccountDTO?> AddAsync(AccountDTO? obj) => await _repository.AddAsync(obj);
    public async Task<List<AccountDTO>> AddRangeAsync(IEnumerable<AccountDTO> objs) => await _repository.AddRangeAsync(objs);
    public async Task<AccountDTO?> UpdateAsync(AccountDTO obj)
    {
        obj.Username = obj.Username.ToLowerInvariant();
        obj.Email = obj.Email.ToLowerInvariant();
        return await _repository.UpdateAsync(obj);
    } 
    public async Task<List<AccountDTO>> UpdateRangeAsync(IEnumerable<AccountDTO> objs)
    {
        foreach (var accountDto in objs)
        {
            accountDto.Username = accountDto.Username.ToLowerInvariant();
            accountDto.Email = accountDto.Email.ToLowerInvariant();
        }

        return await _repository.UpdateRangeAsync(objs);
    }
    public async Task DeleteByIdAsync(Guid id) => await _repository.DeleteByIdAsync(id);
    public async Task DeleteByIdsAsync(IEnumerable<Guid> ids) => await _repository.DeleteByIdsAsync(ids);
}