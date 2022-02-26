using Backend.Libs.Database.Account;
using Backend.Libs.Grpc.Account;
using Backend.Libs.Grpc.CustomTypes;
using Google.Protobuf.WellKnownTypes;
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

    public override async Task<GetAccountByIdResponse> GetAccountById(GetAccountByIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received for id: {Id}", nameof(GetAccountById), request.Id.Value);
        
        AccountDTO? accountDto = await _accountDao.GetByIdAsync(request.Id);
        
        return new GetAccountByIdResponse
        { 
            GrpcAccountDto = accountDto == null ? new GrpcAccountDTO() : accountDto,
            ResponseType = accountDto == null ? GrpcResponseType.ServerError : GrpcResponseType.Success
        };
    }

    public override async Task<AddAccountResponse> AddAccount(AddAccountRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[{Scope}] gRPC request has been received", nameof(AddAccount));

        if (request.GrpcAccountDto == null)
        {
            return new AddAccountResponse { ResponseType = GrpcResponseType.ServerError };
        }

        AccountDTO? accountDto = await _accountDao.AddAsync(request.GrpcAccountDto);

        return new AddAccountResponse
        {
            GrpcAccountDto = accountDto == null ? new GrpcAccountDTO() : accountDto,
            ResponseType = accountDto == null ? GrpcResponseType.ServerError : GrpcResponseType.Success
        };
    }
}