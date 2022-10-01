using Backend.Libs.Database.Account;
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
    private readonly IKeyValueAsyncStorage<AccountDTO, Guid> _accountByIdStorage;
    private readonly ILogger<AccountService> _logger;
    private readonly IGrpcAccountService _grpcService;
    private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(2);

    public AccountService(IKeyValueAsyncStorage<AccountDTO, Guid> accountByIdStorage, ILogger<AccountService> logger, IGrpcAccountService grpcService)
    {
        _accountByIdStorage = accountByIdStorage;
        _logger = logger;
        _grpcService = grpcService;
    }

    public async Task<ServiceResult<AccountDTO?>> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool forceRefresh = false)
    {
        try
        {
            if (!forceRefresh)
            {
                AccountDTO? accountDto = await _accountByIdStorage.GetByIdAsync(id);
                if (accountDto != default)
                {
                    return new ServiceResult<AccountDTO?>(accountDto);
                }
            }

            GrpcAccountResponse response = await _grpcService.GetAccountByIdAsync(new GrpcGetAccountByIdRequest { Id = id }, cancellationToken);

            if (response.Type is GrpcResponseType.RequestError or GrpcResponseType.UnknownError)
            {
                return new ServiceResult<AccountDTO?>(ServiceResultType.Error);
            }

            if (response.Type == GrpcResponseType.NotFound || response.AccountDto == null)
            {
                return new ServiceResult<AccountDTO?>(ServiceResultType.NotFound);
            }

            await _accountByIdStorage.RegisterAsync(id, response.AccountDto, _cacheTtl);

            return new ServiceResult<AccountDTO?>(response.AccountDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetByIdAsync));
            return new ServiceResult<AccountDTO?>(ServiceResultType.Maintenance);
        }
    }

    public async Task<ServiceResult> UpdateAsync(AccountDTO obj, CancellationToken cancellationToken)
    {
        try
        {
            GrpcAccountResponse response = await _grpcService.UpdateAccountAsync(new GrpcSaveAccountRequest { AccountDto = obj }, cancellationToken);

            if (response.Type is GrpcResponseType.UnknownError or GrpcResponseType.RequestError)
            {
                return new ServiceResult(ServiceResultType.Error);
            }

            if (response.Type == GrpcResponseType.Failure || response.AccountDto == null)
            {
                return new ServiceResult(ServiceResultType.Failure);
            }

            await _accountByIdStorage.RegisterAsync(response.AccountDto.Id, response.AccountDto, _cacheTtl);

            return new ServiceResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(UpdateAsync));
            return new ServiceResult<AccountDTO?>(ServiceResultType.Maintenance);
        }
    }

    public async Task<ServiceResult<AccountDTO?>> AddAsync(AccountDTO obj, CancellationToken cancellationToken)
    {
        try
        {
            GrpcAccountResponse response = await _grpcService.AddAccountAsync(new GrpcSaveAccountRequest { AccountDto = obj }, cancellationToken);

            if (response.Type is GrpcResponseType.UnknownError or GrpcResponseType.RequestError)
            {
                return new ServiceResult<AccountDTO?>(ServiceResultType.Error);
            }

            if (response.Type == GrpcResponseType.Failure || response.AccountDto == null)
            {
                return new ServiceResult<AccountDTO?>(ServiceResultType.NotFound);
            }

            await _accountByIdStorage.RegisterAsync(response.AccountDto.Id, response.AccountDto, _cacheTtl);

            return new ServiceResult<AccountDTO?>(response.AccountDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(AddAsync));
            return new ServiceResult<AccountDTO?>(ServiceResultType.Maintenance);
        }
    }

    public Task<ServiceResult> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}