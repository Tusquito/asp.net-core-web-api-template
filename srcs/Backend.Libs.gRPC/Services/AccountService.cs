using Backend.Libs.Database.Account;
using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Account.Request;
using Backend.Libs.gRPC.Account.Responses;
using Backend.Libs.gRPC.Enums;
using Microsoft.Extensions.Logging;

namespace Backend.Libs.gRPC.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountDAO _accountDao;
    public AccountService(ILogger<AccountService> logger, IAccountDAO accountDao)
    {
        _logger = logger;
        _accountDao = accountDao;
    }

    public async Task<GrpcAccountResponse> GetAccountByIdAsync(GrpcGetAccountByIdRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received for id: {Id}", nameof(GetAccountByIdAsync), request.Id);
        
        AccountDTO? accountDto = await _accountDao.GetByIdAsync(request.Id);
        
        return new GrpcAccountResponse
        { 
            AccountDto = accountDto,
            Type = accountDto == null ? GrpcResponseType.Failed : GrpcResponseType.Success
        };
    }

    public async Task<GrpcAccountResponse> GetAccountByEmailAsync(GrpcGetAccountByStringRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received for email: {Email}", nameof(GetAccountByEmailAsync), request.Search);
        
        AccountDTO? accountDto = await _accountDao.GetByEmailAsync(request.Search);
        
        return new GrpcAccountResponse
        { 
            AccountDto = accountDto,
            Type = accountDto == null ? GrpcResponseType.Failed : GrpcResponseType.Success
        };
    }

    public async Task<GrpcAccountResponse> GetAccountByUsernameAsync(GrpcGetAccountByStringRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received for username: {Username}", nameof(GetAccountByUsernameAsync), request.Search);
        
        AccountDTO? accountDto = await _accountDao.GetByUsernameAsync(request.Search);
        
        return new GrpcAccountResponse
        { 
            AccountDto = accountDto,
            Type = accountDto == null ? GrpcResponseType.Failed : GrpcResponseType.Success
        };
    }

    public async Task<GrpcAccountResponse> UpdateAccountAsync(GrpcSaveAccountRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received", nameof(UpdateAccountAsync));

        if (request.AccountDto == null)
        {
            return new GrpcAccountResponse { Type = GrpcResponseType.UnknownError };
        }

        AccountDTO? accountDto = await _accountDao.UpdateAsync(request.AccountDto);

        return new GrpcAccountResponse
        {
            AccountDto = accountDto,
            Type = accountDto == null ? GrpcResponseType.Failed : GrpcResponseType.Success
        };
    }

    public async Task<GrpcAccountResponse> AddAccountAsync(GrpcSaveAccountRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received", nameof(AddAccountAsync));

        if (request.AccountDto == null)
        {
            return new GrpcAccountResponse { Type = GrpcResponseType.InternalServerError };
        }

        AccountDTO? accountDto = await _accountDao.AddAsync(request.AccountDto);

        return new GrpcAccountResponse
        {
            AccountDto = accountDto,
            Type = accountDto == null ? GrpcResponseType.Failed : GrpcResponseType.Success
        };
    }
}