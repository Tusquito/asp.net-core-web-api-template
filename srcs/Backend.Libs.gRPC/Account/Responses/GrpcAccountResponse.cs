using Backend.Libs.Database.Account;
using ProtoBuf;

namespace Backend.Libs.gRPC.Account.Responses;

[ProtoContract]
public class GrpcAccountResponse : GrpcBaseResponse
{
    [ProtoMember(1)]
    public AccountDto? AccountDto { get; init; }
}