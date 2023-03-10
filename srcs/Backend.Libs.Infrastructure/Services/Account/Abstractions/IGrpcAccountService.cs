using System.ServiceModel;
using Backend.Libs.Infrastructure.Requests.Account;
using Backend.Libs.Infrastructure.Responses.Account;

namespace Backend.Libs.Infrastructure.Services.Account.Abstractions;

[ServiceContract]
public interface IGrpcAccountService
{
    [OperationContract]
    Task<GrpcAccountResponse> GetAccountByIdAsync(GrpcGetAccountByIdRequest request, CancellationToken cancellationToken);
    
    [OperationContract]
    Task<GrpcAccountResponse> GetAccountByEmailAsync(GrpcGetAccountByStringRequest request, CancellationToken cancellationToken);

    [OperationContract]
    Task<GrpcAccountResponse> GetAccountByUsernameAsync(GrpcGetAccountByStringRequest request,
        CancellationToken cancellationToken);

    [OperationContract]
    Task<GrpcAccountResponse> UpdateAccountAsync(GrpcSaveAccountRequest request, CancellationToken cancellationToken);

    [OperationContract]
    Task<GrpcAccountResponse> AddAccountAsync(GrpcSaveAccountRequest request, CancellationToken cancellationToken);

}