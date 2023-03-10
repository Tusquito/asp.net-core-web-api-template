using ProtoBuf;

namespace Backend.Libs.Infrastructure.Enums;

[ProtoContract]
public enum GrpcResponseType
{
    Success,
    Failure,
    NotFound,
    RequestError,
    UnknownError
}