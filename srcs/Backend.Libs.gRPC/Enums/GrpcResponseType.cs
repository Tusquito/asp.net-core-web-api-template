using ProtoBuf;

namespace Backend.Libs.gRPC.Enums;

[ProtoContract]
public enum GrpcResponseType
{
    Success,
    Failure,
    NotFound,
    RequestError,
    UnknownError
}