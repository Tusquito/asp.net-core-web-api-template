using ProtoBuf;

namespace Backend.Libs.gRPC.Account.Request;

[ProtoContract]
public class GrpcGetAccountByStringRequest
{
    [ProtoMember(1)]
    public required string Search { get; init; }
}