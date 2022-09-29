using ProtoBuf;

namespace Backend.Libs.gRPC.Enums;

[ProtoContract]
public enum GrpcResponseType
{
    UnknownError,
    Success,
    Failed,
    InternalServerError,
}