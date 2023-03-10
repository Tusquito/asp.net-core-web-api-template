using Backend.Libs.Infrastructure.Enums;
using ProtoBuf;

namespace Backend.Libs.Infrastructure.Responses;

[ProtoContract]
public class GrpcBaseResponse
{
    [ProtoMember(1)] 
    public GrpcResponseType Type { get; init; }
}