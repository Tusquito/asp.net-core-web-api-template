using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Libs.Domain;

public record GenericResult(int Status, object? Data, GenericResultMessage[] Messages)
{
    public GenericResult(int status, object? data, GenericResultMessage message) : this(status, data, new []{message}){}
}

public record GenericResultMessage(ResultMessageKey Key, int Code, params object[] Args)
{
    public GenericResultMessage(ResultMessageKey key, params object[] args) : this(key, (int)key, args) {}
}
    
public static class DomainResult
{
    public static OkObjectResult Ok(object? data = null, ResultMessageKey messageKey = ResultMessageKey.Ok, params object[] args)
    {
        return new OkObjectResult(new GenericResult(StatusCodes.Status200OK, data, new GenericResultMessage(messageKey, args)));
    }
    
    public static OkObjectResult Ok<T>(Result<T> result,  params object[] args)
    {
        return Ok(result.Value, result.Message, args);
    }
    
    public static OkObjectResult Ok(Result result,  params object[] args)
    {
        return Ok(null, messageKey:result.Message, args);
    }

    public static CreatedObjectResult Created(object? data = null, ResultMessageKey messageKey = ResultMessageKey.Created, params object[] args)
    {
        return new CreatedObjectResult(new GenericResult(StatusCodes.Status201Created, data, new GenericResultMessage(messageKey, args)));
    }
    
    public static CreatedObjectResult Created<T>(Result<T> result, params object[] args)
    {
        return Created(result.Value, result.Message, args);
    }

    public static CreatedObjectResult Created(Result result, params object[] args)
    {
        return Created(null, messageKey:result.Message, args);
    }

    public static NoContentResult NoContent()
    {
        return new NoContentResult();
    }

    public static BadRequestObjectResult BadRequest(ResultMessageKey messageKey = ResultMessageKey.BadRequest, params object[] args)
    {
        return new BadRequestObjectResult(new GenericResult(StatusCodes.Status400BadRequest, null, new GenericResultMessage(messageKey, args)));
    }
    
    public static BadRequestObjectResult BadRequest(IEnumerable<ResultMessageKey> messageKeys)
    {
        return new BadRequestObjectResult(new GenericResult(StatusCodes.Status400BadRequest, null, messageKeys.Select(x => new GenericResultMessage(x)).ToArray()));
    }
    
    public static NotFoundObjectResult NotFound(ResultMessageKey messageKey = ResultMessageKey.NotFound, params object[] args)
    {
        return new NotFoundObjectResult(new GenericResult(StatusCodes.Status404NotFound, null, new GenericResultMessage(messageKey, args)));
    }
    
    public static NotFoundObjectResult NotFound<T>(Result<T> result, params object[] args)
    {
        return NotFound(result.Message, args);
    }
    
    public static NotFoundObjectResult NotFound(Result result, params object[] args)
    {
        return NotFound(result.Message, args);
    }
    
    public static UnauthorizedObjectResult Unauthorized(ResultMessageKey messageKey = ResultMessageKey.Unauthorized, params object[] args)
    {
        return new UnauthorizedObjectResult(new GenericResult(StatusCodes.Status401Unauthorized, null, new GenericResultMessage(messageKey, args)));
    }
    
    public static UnauthorizedObjectResult Unauthorized<T>(Result<T> result, params object[] args)
    {
        return Unauthorized(result.Message, args);
    }
    
    public static UnauthorizedObjectResult Unauthorized(Result result, params object[] args)
    {
        return Unauthorized(result.Message, args);
    }
    
    public static InternalServerErrorObjectResult InternalServerError(ResultMessageKey messageKey = ResultMessageKey.InternalServerError, params object[] args)
    {
        return new InternalServerErrorObjectResult(new GenericResult(StatusCodes.Status500InternalServerError, null, new GenericResultMessage(messageKey, args)));
    }
    
    public static InternalServerErrorObjectResult InternalServerError<T>(Result<T> result, params object[] args)
    {
        return InternalServerError(result.Message, args);
    }
    
    public static InternalServerErrorObjectResult InternalServerError(Result result, params object[] args)
    {
        return InternalServerError(result.Message, args);
    }
    
    public static ServiceUnavailableObjectResult ServiceUnavailable(ResultMessageKey messageKey = ResultMessageKey.ServiceUnavailable, params object[] args)
    {
        return new ServiceUnavailableObjectResult(new GenericResult(StatusCodes.Status503ServiceUnavailable, null, new GenericResultMessage(messageKey, args)));
    }
    
    public static ServiceUnavailableObjectResult ServiceUnavailable<T>(Result<T> result, params object[] args)
    {
        return ServiceUnavailable(result.Message, args);
    }
    
    public static ServiceUnavailableObjectResult ServiceUnavailable(Result result, params object[] args)
    {
        return ServiceUnavailable(result.Message, args);
    }
}