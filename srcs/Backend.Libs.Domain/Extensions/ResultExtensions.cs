using System;
using Backend.Libs.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Libs.Domain.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        return result.Type switch
        {
            ResultType.Maintenance => DomainResults.ServiceUnavailable(result.Message),
            ResultType.Success => DomainResults.Ok(result.Message),
            ResultType.UnknownError => DomainResults.InternalServerError(result.Message),
            ResultType.BadRequest => DomainResults.BadRequest(result.Message),
            ResultType.NotFound => DomainResults.NotFound(result.Message),
            ResultType.Failure => DomainResults.InternalServerError(result.Message),
            ResultType.Created => DomainResults.Created(result.Message),
            ResultType.Ok => DomainResults.Ok(result.Message),
            ResultType.NoContent => DomainResults.NoContent(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.Type switch
        {
            ResultType.Maintenance => DomainResults.ServiceUnavailable(result.Message),
            ResultType.Success => DomainResults.Ok(result.Value, result.Message),
            ResultType.UnknownError => DomainResults.InternalServerError(result.Message),
            ResultType.BadRequest => DomainResults.BadRequest(result.Message),
            ResultType.NotFound => DomainResults.NotFound(result.Message),
            ResultType.Failure => DomainResults.InternalServerError(result.Message),
            ResultType.Created => DomainResults.Created(result.Value, result.Message),
            ResultType.Ok => DomainResults.Ok(result.Value, result.Message),
            ResultType.NoContent => DomainResults.NoContent(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}