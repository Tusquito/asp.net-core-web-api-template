using Backend.Libs.Database.Account;
using Backend.Libs.Database.Generic;
using Backend.Plugins.Database.Context;
using Backend.Plugins.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Plugins.Database.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IGenericMapper<AccountEntity, AccountDto?> _mapper;
    private readonly IDbContextFactory<BackendDbContext> _contextFactory;
    private readonly IGenericAsyncUuidRepository<AccountDto> _repository;
    private readonly ILogger<AccountRepository> _logger;

    public AccountRepository(IGenericMapper<AccountEntity, AccountDto?> mapper, IDbContextFactory<BackendDbContext> contextFactory,
        IGenericAsyncUuidRepository<AccountDto> repository, ILogger<AccountRepository> logger)
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
        _repository = repository;
        _logger = logger;
    }

    public async Task<AccountDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return _mapper.Map(await context.Accounts.FirstOrDefaultAsync(x => x.Username.Equals(username.ToLowerInvariant()), cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(GetByUsernameAsync)}]");
            return null;
        }
    }

    public async Task<AccountDto?> GetAccountByIpAsync(string ip, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return _mapper.Map(await context.Accounts.FirstOrDefaultAsync(x => x.Ip.Equals(ip), cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(GetAccountByIpAsync)}]");
            return null;
        }
    }

    public async Task<AccountDto?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            await using BackendDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return _mapper.Map(await context.Accounts.FirstOrDefaultAsync(x => x.Email.Equals(email.ToLower()), cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(GetByEmailAsync)}]");
            return null;
        }
    }

    public async Task<List<AccountDto?>?> GetAllAsync(CancellationToken cancellationToken) => await _repository.GetAllAsync(cancellationToken);
    public async Task<AccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await _repository.GetByIdAsync(id, cancellationToken);

    public async Task<List<AccountDto?>?> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken) =>
        await _repository.GetByIdsAsync(ids, cancellationToken);

    public async Task<AccountDto?> AddAsync(AccountDto obj, CancellationToken cancellationToken) => await _repository.AddAsync(obj, cancellationToken);

    public async Task<List<AccountDto>?> AddRangeAsync(IEnumerable<AccountDto> objs, CancellationToken cancellationToken) =>
        await _repository.AddRangeAsync(objs, cancellationToken);

    public async Task<AccountDto?> UpdateAsync(AccountDto obj, CancellationToken cancellationToken)
    {
        obj.Username = obj.Username.ToLowerInvariant();
        obj.Email = obj.Email.ToLowerInvariant();
        return await _repository.UpdateAsync(obj, cancellationToken);
    }

    public async Task<List<AccountDto>?> UpdateRangeAsync(IEnumerable<AccountDto> objs, CancellationToken cancellationToken)
    {
        foreach (var accountDto in objs)
        {
            accountDto.Username = accountDto.Username.ToLowerInvariant();
            accountDto.Email = accountDto.Email.ToLowerInvariant();
        }

        return await _repository.UpdateRangeAsync(objs, cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken) => await _repository.DeleteByIdAsync(id, cancellationToken);

    public async Task DeleteByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken) =>
        await _repository.DeleteByIdsAsync(ids, cancellationToken);
}