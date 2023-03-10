using Backend.Libs.Persistence.Data.Account;
using ProtoBuf;

namespace Backend.Libs.Infrastructure.Requests.Account;

[ProtoContract]
public class GrpcSaveAccountRequest
{
    [ProtoMember(1)]
    public AccountDto? AccountDto { get; init; }
}