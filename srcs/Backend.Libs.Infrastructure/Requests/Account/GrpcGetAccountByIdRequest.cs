using ProtoBuf;

namespace Backend.Libs.Infrastructure.Requests.Account;

[ProtoContract]
public class GrpcGetAccountByIdRequest
{
    [ProtoMember(1)]
    public Guid Id { get; init; }
}