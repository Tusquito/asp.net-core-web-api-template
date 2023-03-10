using Backend.Libs.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Libs.Domain.Extensions;

public static class DomainResultExtensions
{
    public static ActionResult FromResult<T>(this Result<T> result)
    {
        return result.Type switch
        {
            ResultType.Maintenance => DomainResult.ServiceUnavailable(result),
            ResultType.Ok => DomainResult.Ok(result),
            ResultType.Created => DomainResult.Created(result),
            ResultType.NoContent => DomainResult.NoContent(),
            ResultType.UnknownError => DomainResult.InternalServerError(result),
            ResultType.BadRequest => DomainResult.BadRequest(),
            ResultType.NotFound => DomainResult.NotFound(result),
            ResultType.Success => DomainResult.Ok(result),
            ResultType.Failure => DomainResult.InternalServerError(result),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static ActionResult FromResult(this Result result)
    {
        return result.Type switch
        {
            ResultType.Maintenance => DomainResult.ServiceUnavailable(result),
            ResultType.Ok => DomainResult.Ok(result),
            ResultType.Created => DomainResult.Created(result),
            ResultType.NoContent => DomainResult.NoContent(),
            ResultType.UnknownError => DomainResult.InternalServerError(result),
            ResultType.BadRequest => DomainResult.BadRequest(),
            ResultType.NotFound => DomainResult.NotFound(result),
            ResultType.Success => DomainResult.Ok(result),
            ResultType.Failure => DomainResult.InternalServerError(result),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}