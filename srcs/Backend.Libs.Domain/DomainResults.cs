using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.ObjectResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Libs.Domain;

public record GenericResult(int Status, object? Data, GenericResultMessage Message);

public record GenericResultMessage(ResultMessageKey Key, int Code, params object[] Args);
    
public static class DomainResults
{
    public static OkObjectResult Ok(object? data = null, ResultMessageKey messageKey = ResultMessageKey.Ok, params object[] args)
    {
        return new OkObjectResult(new GenericResult(StatusCodes.Status200OK, data, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
    
    public static CreatedObjectResult Created(object? data = null, ResultMessageKey messageKey = ResultMessageKey.Created, params object[] args)
    {
        return new CreatedObjectResult(new GenericResult(StatusCodes.Status201Created, data, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }

    public static NoContentResult NoContent()
    {
        return new NoContentResult();
    }
    
    
    
    public static BadRequestObjectResult BadRequest(ResultMessageKey messageKey = ResultMessageKey.BadRequest, params object[] args)
    {
        return new BadRequestObjectResult(new GenericResult(StatusCodes.Status400BadRequest, null, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
    
    public static NotFoundObjectResult NotFound(ResultMessageKey messageKey = ResultMessageKey.NotFound, params object[] args)
    {
        return new NotFoundObjectResult(new GenericResult(StatusCodes.Status404NotFound, null, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
    
    public static UnauthorizedObjectResult Unauthorized(ResultMessageKey messageKey = ResultMessageKey.Unauthorized, params object[] args)
    {
        return new UnauthorizedObjectResult(new GenericResult(StatusCodes.Status401Unauthorized, null, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
    
    public static InternalServerErrorObjectResult InternalServerError(ResultMessageKey messageKey = ResultMessageKey.InternalServerError, params object[] args)
    {
        return new InternalServerErrorObjectResult(new GenericResult(StatusCodes.Status500InternalServerError, null, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
    
    public static ServiceUnavailableObjectResult ServiceUnavailable(ResultMessageKey messageKey = ResultMessageKey.ServiceUnavailable, params object[] args)
    {
        return new ServiceUnavailableObjectResult(new GenericResult(StatusCodes.Status503ServiceUnavailable, null, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
}