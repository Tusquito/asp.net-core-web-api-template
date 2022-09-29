using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Libs.Domain;

internal record GenericResult(int Status, object Data, GenericResultMessage Message);
internal record GenericResultMessage(ResultMessageKey Key, int Code, string[] Params);
    
public static class EndpointResult
{
    public static OkObjectResult Ok(object data = null, ResultMessageKey messageKey = ResultMessageKey.SUCCESS, string[] @params = null)
    {
        return new OkObjectResult(new GenericResult(StatusCodes.Status200OK, data, new GenericResultMessage(messageKey, (int)messageKey, @params)));
    }
    
    public static BadRequestObjectResult BadRequest(ResultMessageKey messageKey = ResultMessageKey.BAD_REQUEST, string[] @params = null)
    {
        return new BadRequestObjectResult(new GenericResult(StatusCodes.Status400BadRequest, null, new GenericResultMessage(messageKey, (int)messageKey, @params)));
    }
    
    public static NotFoundObjectResult NotFound(ResultMessageKey messageKey = ResultMessageKey.NOT_FOUND, string[] @params = null)
    {
        return new NotFoundObjectResult(new GenericResult(StatusCodes.Status404NotFound, null, new GenericResultMessage(messageKey, (int)messageKey, @params)));
    }
    
    public static UnauthorizedObjectResult Unauthorized(ResultMessageKey messageKey = ResultMessageKey.UNAUTHORIZED, string[] @params = null)
    {
        return new UnauthorizedObjectResult(new GenericResult(StatusCodes.Status401Unauthorized, null, new GenericResultMessage(messageKey, (int)messageKey, @params)));
    }
    
    public static InternalServerErrorObjectResult InternalServerError(ResultMessageKey messageKey = ResultMessageKey.INTERNAL_SERVER_ERROR, string[] @params = null)
    {
        return new InternalServerErrorObjectResult(new GenericResult(StatusCodes.Status500InternalServerError, null, new GenericResultMessage(messageKey, (int)messageKey, @params)));
    }
}