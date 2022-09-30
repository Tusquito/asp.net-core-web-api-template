using System.ServiceModel;
using Backend.Libs.gRPC.Account.Request;
using Backend.Libs.gRPC.Account.Responses;

namespace Backend.Libs.gRPC.Account;

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