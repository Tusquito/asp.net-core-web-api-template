using ProtoBuf;

namespace Backend.Libs.gRPC.Enums;

[ProtoContract]
public enum GrpcResponseType
{
    Success,
    Failed,
    NotFound,
    RequestError,
    UnknownError
}