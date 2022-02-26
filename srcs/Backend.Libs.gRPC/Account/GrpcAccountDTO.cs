using Backend.Libs.Database.Account;
using Mapster;

namespace Backend.Libs.Grpc.Account;

public partial class GrpcAccountDTO
{
    public static implicit operator AccountDTO(GrpcAccountDTO grpcAccountDto)
    {
        return grpcAccountDto.Adapt<AccountDTO>();
    }

    public static implicit operator GrpcAccountDTO(AccountDTO accountDto)
    {
        return accountDto.Adapt<GrpcAccountDTO>();
    }
}