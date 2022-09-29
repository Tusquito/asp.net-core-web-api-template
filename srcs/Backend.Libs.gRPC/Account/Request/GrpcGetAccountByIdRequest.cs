using ProtoBuf;

namespace Backend.Libs.gRPC.Account.Request;

[ProtoContract]
public class GrpcGetAccountByIdRequest
{
    [ProtoMember(1)]
    public Guid Id { get; init; }
}