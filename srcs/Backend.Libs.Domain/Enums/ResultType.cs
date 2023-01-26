using System;

namespace Backend.Libs.Domain.Enums;

[Flags]
public enum ResultType : byte
{
    Maintenance = 0,
    Ok = 1 << 0,
    Created = 1 << 1,
    NoContent = 1 << 2,
    UnknownError = 1 << 3,
    BadRequest = 1 << 4,
    NotFound = 1 << 5,
    
    Success = Ok | Created | NoContent,
    Failure = UnknownError | BadRequest | NotFound | Maintenance
}