using Backend.Libs.Database.Account;
using Backend.Libs.Grpc.Account;
using Backend.Libs.Grpc.CustomTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Backend.Libs.gRPC.Services;

public class AccountService : GrpcAccountService.GrpcAccountServiceBase
{
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountDAO _accountDao;
    public AccountService(ILogger<AccountService> logger, IAccountDAO accountDao)
    {
        _logger = logger;
        _accountDao = accountDao;
    }

    public override async Task<AccountResponse> GetAccountById(GetAccountByIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received for id: {Id}", nameof(GetAccountById), request.Id.Value);
        
        AccountDTO? accountDto = await _accountDao.GetByIdAsync(request.Id);
        
        return new AccountResponse
        { 
            GrpcAccountDto = accountDto == null ? new GrpcAccountDTO() : accountDto,
            ResponseType = accountDto == null ? GrpcResponseType.ServerError : GrpcResponseType.Success
        };
    }

    public override async Task<AccountResponse> GetAccountByEmail(GetAccountByStringRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received for email: {Email}", nameof(GetAccountById), request.Search);
        
        AccountDTO? accountDto = await _accountDao.GetByEmailAsync(request.Search);
        
        return new AccountResponse
        { 
            GrpcAccountDto = accountDto == null ? new GrpcAccountDTO() : accountDto,
            ResponseType = accountDto == null ? GrpcResponseType.ServerError : GrpcResponseType.Success
        };
    }

    public override async Task<AccountResponse> GetAccountByUsername(GetAccountByStringRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received for username: {Username}", nameof(GetAccountById), request.Search);
        
        AccountDTO? accountDto = await _accountDao.GetByUsernameAsync(request.Search);
        
        return new AccountResponse
        { 
            GrpcAccountDto = accountDto == null ? new GrpcAccountDTO() : accountDto,
            ResponseType = accountDto == null ? GrpcResponseType.ServerError : GrpcResponseType.Success
        };
    }

    public override async Task<AccountResponse> UpdateAccount(SaveAccountRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received", nameof(UpdateAccount));

        if (request.GrpcAccountDto == null)
        {
            return new AccountResponse { ResponseType = GrpcResponseType.ServerError };
        }

        AccountDTO? accountDto = await _accountDao.UpdateAsync(request.GrpcAccountDto);

        return new AccountResponse
        {
            GrpcAccountDto = accountDto == null ? new GrpcAccountDTO() : accountDto,
            ResponseType = accountDto == null ? GrpcResponseType.ServerError : GrpcResponseType.Success
        };
    }

    public override async Task<AccountResponse> AddAccount(AddAccountRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received", nameof(AddAccount));

        if (request.GrpcAccountDto == null)
        {
            return new AccountResponse { ResponseType = GrpcResponseType.ServerError };
        }

        AccountDTO? accountDto = await _accountDao.AddAsync(request.GrpcAccountDto);

        return new AccountResponse
        {
            GrpcAccountDto = accountDto == null ? new GrpcAccountDTO() : accountDto,
            ResponseType = accountDto == null ? GrpcResponseType.ServerError : GrpcResponseType.Success
        };
    }
}