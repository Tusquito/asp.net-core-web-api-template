using Backend.Libs.Database.Account;
using ProtoBuf;

namespace Backend.Libs.gRPC.Account.Request;

[ProtoContract]
public class GrpcSaveAccountRequest
{
    [ProtoMember(1)]
    public AccountDto? AccountDto { get; init; }
}