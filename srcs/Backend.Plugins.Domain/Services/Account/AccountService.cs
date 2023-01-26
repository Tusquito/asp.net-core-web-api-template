using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Services.Account;
using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Account.Request;
using Backend.Libs.gRPC.Account.Responses;
using Backend.Libs.gRPC.Enums;
using Backend.Libs.Redis;
using Microsoft.Extensions.Logging;

namespace Backend.Plugins.Domain.Services.Account;

public class AccountService : IAccountService
{
    private readonly IKeyValueAsyncStorage<AccountDto, Guid> _accountByIdStorage;
    private readonly IKeyValueAsyncStorage<AccountDto, string> _accountByLoginStorage;
    private readonly ILogger<AccountService> _logger;
    private readonly IGrpcAccountService _grpcService;
    private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(2);

    public AccountService(IKeyValueAsyncStorage<AccountDto, Guid> accountByIdStorage, ILogger<AccountService> logger, IGrpcAccountService grpcService, IKeyValueAsyncStorage<AccountDto, string> accountByLoginStorage)
    {
        _accountByIdStorage = accountByIdStorage;
        _logger = logger;
        _grpcService = grpcService;
        _accountByLoginStorage = accountByLoginStorage;
    }

    public async Task<Result<AccountDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool forceRefresh = false)
    {
        try
        {
            if (!forceRefresh)
            {
                AccountDto? accountDto = await _accountByIdStorage.GetByIdAsync(id);
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

            await _accountByIdStorage.RegisterAsync(id, response.AccountDto, _cacheTtl);

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

            await _accountByIdStorage.RegisterAsync(response.AccountDto.Id, response.AccountDto, _cacheTtl);
            await _accountByLoginStorage.RegisterAsync(response.AccountDto.Email, response.AccountDto, _cacheTtl);
            await _accountByLoginStorage.RegisterAsync(response.AccountDto.Username, response.AccountDto, _cacheTtl);

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
                return Result<AccountDto?>.Failed();
            }

            await _accountByIdStorage.RegisterAsync(response.AccountDto.Id, response.AccountDto, _cacheTtl);
            await _accountByLoginStorage.RegisterAsync(response.AccountDto.Email, response.AccountDto, _cacheTtl);
            await _accountByLoginStorage.RegisterAsync(response.AccountDto.Username, response.AccountDto, _cacheTtl);

            return Result<AccountDto?>.Created(response.AccountDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(AddAsync));
            return Result<AccountDto?>.Maintenance();
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
                AccountDto? accountDto = await _accountByLoginStorage.GetByIdAsync(email);
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

            await _accountByLoginStorage.RegisterAsync(email, response.AccountDto, _cacheTtl);

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
                AccountDto? accountDto = await _accountByLoginStorage.GetByIdAsync(username);
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

            await _accountByLoginStorage.RegisterAsync(username, response.AccountDto, _cacheTtl);

            return Result<AccountDto?>.Ok(response.AccountDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetByUsernameAsync));
            return Result<AccountDto?>.Maintenance();
        }
    }
}