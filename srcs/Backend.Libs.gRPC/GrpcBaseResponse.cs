using Backend.Libs.gRPC.Enums;
using ProtoBuf;

namespace Backend.Libs.gRPC;

[ProtoContract]
public class GrpcBaseResponse
{
    [ProtoMember(1)] 
    public GrpcResponseType Type { get; init; }
}