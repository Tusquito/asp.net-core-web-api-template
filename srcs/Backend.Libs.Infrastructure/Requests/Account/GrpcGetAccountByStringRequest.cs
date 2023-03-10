using ProtoBuf;

namespace Backend.Libs.Infrastructure.Requests.Account;

[ProtoContract]
public class GrpcGetAccountByStringRequest
{
    [ProtoMember(1)]
    public required string Search { get; init; }
}