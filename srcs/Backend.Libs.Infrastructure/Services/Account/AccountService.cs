using Backend.Libs.Domain;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Infrastructure.Enums;
using Backend.Libs.Infrastructure.Repositories.Abstractions;
using Backend.Libs.Infrastructure.Requests.Account;
using Backend.Libs.Infrastructure.Responses.Account;
using Backend.Libs.Infrastructure.Services.Account.Abstractions;
using Backend.Libs.Persistence.Data.Account;
using Microsoft.Extensions.Logging;

namespace Backend.Libs.Infrastructure.Services.Account;

public class AccountService : IAccountService
{
    private readonly IKeyValueRepository<Guid, AccountDto> _accountByIdCache;
    private readonly IKeyValueRepository<string, AccountDto> _accountByLoginCache;
    private readonly ILogger<AccountService> _logger;
    private readonly IGrpcAccountService _grpcService;
    private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(2);

    public AccountService(IKeyValueRepository<Guid, AccountDto> accountByIdCache, ILogger<AccountService> logger,
        IGrpcAccountService grpcService, IKeyValueRepository<string, AccountDto> accountByLoginCache)
    {
        _accountByIdCache = accountByIdCache;
        _logger = logger;
        _grpcService = grpcService;
        _accountByLoginCache = accountByLoginCache;
    }

    public async Task<Result<AccountDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool forceRefresh = false)
    {
        try
        {
            if (!forceRefresh)
            {
                AccountDto? accountDto = await _accountByIdCache.GetByIdAsync(id);
                if (accountDto != default)
                {
                    return Result<AccountDto?>.Ok(accountDto);
                }
            }

            GrpcAccountResponse response = await _grpcService.GetAccountByIdAsync(new GrpcGetAccountByIdRequest { Id = id }, cancellationToken);

            if (response.Type is GrpcResponseType.RequestError or GrpcResponseType.UnknownError)
            {
                return Result<AccountDto?>.BadRequest();
            }

            if (response.Type == GrpcResponseType.NotFound || response.AccountDto == null)
            {
                return Result<AccountDto?>.NotFound(ResultMessageKey.NotFoundAccountById);
            }

            await _accountByIdCache.RegisterAsync(id, response.AccountDto, _cacheTtl);

            return Result<AccountDto?>.Ok(response.AccountDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetByIdAsync));
            return Result<AccountDto?>.Maintenance();
        }
    }

    public async Task<Result> UpdateAsync(AccountDto obj, CancellationToken cancellationToken)
    {
        try
        {
            GrpcAccountResponse response = await _grpcService.UpdateAccountAsync(new GrpcSaveAccountRequest { AccountDto = obj }, cancellationToken);

            if (response.Type is GrpcResponseType.UnknownError or GrpcResponseType.RequestError)
            {
                return Result.Failure();
            }

            if (response.Type == GrpcResponseType.Failure || response.AccountDto == null)
            {
                return Result.NotFound();
            }

            await _accountByIdCache.RegisterAsync(response.AccountDto.Id, response.AccountDto, _cacheTtl);
            await _accountByLoginCache.RegisterAsync(response.AccountDto.Email, response.AccountDto, _cacheTtl);
            await _accountByLoginCache.RegisterAsync(response.AccountDto.Username, response.AccountDto, _cacheTtl);

            return Result.NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(UpdateAsync));
            return Result.Maintenance();
        }
    }

    public async Task<Result<AccountDto?>> AddAsync(AccountDto obj, CancellationToken cancellationToken)
    {
        try
        {
            GrpcAccountResponse response = await _grpcService.AddAccountAsync(new GrpcSaveAccountRequest { AccountDto = obj }, cancellationToken);

            if (response.Type is GrpcResponseType.UnknownError or GrpcResponseType.RequestError)
            {
                return Result<AccountDto?>.BadRequest();
            }

            if (response.Type == GrpcResponseType.Failure || response.AccountDto == null)
            {
                return Result<AccountDto?>.Failure();
            }

            await _accountByIdCache.RegisterAsync(response.AccountDto.Id, response.AccountDto, _cacheTtl);
            await _accountByLoginCache.RegisterAsync(response.AccountDto.Email, response.AccountDto, _cacheTtl);
            await _accountByLoginCache.RegisterAsync(response.AccountDto.Username, response.AccountDto, _cacheTtl);

            return Result<AccountDto?>.Created(response.AccountDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(AddAsync));
            return Result<AccountDto?>.Failure();
        }
    }

    public Task<Result> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<AccountDto?>> GetByEmailAsync(string email, CancellationToken cancellationToken, bool forceRefresh = false)
    {
        try
        {
            if (!forceRefresh)
            {
                AccountDto? accountDto = await _accountByLoginCache.GetByIdAsync(email);
                if (accountDto != default)
                {
                    return Result<AccountDto?>.Ok(accountDto);
                }
            }

            GrpcAccountResponse response = await _grpcService.GetAccountByEmailAsync(new GrpcGetAccountByStringRequest { Search = email }, cancellationToken);

            if (response.Type is GrpcResponseType.RequestError or GrpcResponseType.UnknownError)
            {
                return Result<AccountDto?>.BadRequest();
            }

            if (response.Type == GrpcResponseType.NotFound || response.AccountDto == null)
            {
                return Result<AccountDto?>.NotFound(ResultMessageKey.NotFoundAccountById);
            }

            await _accountByLoginCache.RegisterAsync(email, response.AccountDto, _cacheTtl);

            return Result<AccountDto?>.Ok(response.AccountDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetByEmailAsync));
            return Result<AccountDto?>.Maintenance();
        }
    }

    public async Task<Result<AccountDto?>> GetByUsernameAsync(string username, CancellationToken cancellationToken, bool forceRefresh = false)
    {
        try
        {
            if (!forceRefresh)
            {
                AccountDto? accountDto = await _accountByLoginCache.GetByIdAsync(username);
                if (accountDto != default)
                {
                    return Result<AccountDto?>.Ok(accountDto);
                }
            }

            GrpcAccountResponse response = await _grpcService.GetAccountByUsernameAsync(new GrpcGetAccountByStringRequest { Search = username }, cancellationToken);

            if (response.Type is GrpcResponseType.RequestError or GrpcResponseType.UnknownError)
            {
                return Result<AccountDto?>.BadRequest();
            }

            if (response.Type == GrpcResponseType.NotFound || response.AccountDto == null)
            {
                return Result<AccountDto?>.NotFound(ResultMessageKey.NotFoundAccountByLogin);
            }

            await _accountByLoginCache.RegisterAsync(username, response.AccountDto, _cacheTtl);

            return Result<AccountDto?>.Ok(response.AccountDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetByUsernameAsync));
            return Result<AccountDto?>.Maintenance();
        }
    }
}