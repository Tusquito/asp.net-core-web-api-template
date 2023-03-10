using Backend.Libs.Persistence.Data.Account;
using ProtoBuf;

namespace Backend.Libs.Infrastructure.Responses.Account;

[ProtoContract]
public class GrpcAccountResponse : GrpcBaseResponse
{
    [ProtoMember(1)]
    public AccountDto? AccountDto { get; init; }
}