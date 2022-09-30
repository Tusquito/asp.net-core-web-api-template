using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Libs.Domain;

internal record GenericResult(int Status, object Data, GenericResultMessage Message);
internal record GenericResultMessage(ResultMessageKey Key, int Code, params object[] Args);
    
public static class DomainResults
{
    public static OkObjectResult Ok(object data = null, ResultMessageKey messageKey = ResultMessageKey.SUCCESS, params object[] args)
    {
        return new OkObjectResult(new GenericResult(StatusCodes.Status200OK, data, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
    
    public static BadRequestObjectResult BadRequest(ResultMessageKey messageKey = ResultMessageKey.BAD_REQUEST, params object[] args)
    {
        return new BadRequestObjectResult(new GenericResult(StatusCodes.Status400BadRequest, null, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
    
    public static NotFoundObjectResult NotFound(ResultMessageKey messageKey = ResultMessageKey.NOT_FOUND, params object[] args)
    {
        return new NotFoundObjectResult(new GenericResult(StatusCodes.Status404NotFound, null, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
    
    public static UnauthorizedObjectResult Unauthorized(ResultMessageKey messageKey = ResultMessageKey.UNAUTHORIZED, params object[] args)
    {
        return new UnauthorizedObjectResult(new GenericResult(StatusCodes.Status401Unauthorized, null, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
    
    public static InternalServerErrorObjectResult InternalServerError(ResultMessageKey messageKey = ResultMessageKey.INTERNAL_SERVER_ERROR, params object[] args)
    {
        return new InternalServerErrorObjectResult(new GenericResult(StatusCodes.Status500InternalServerError, null, new GenericResultMessage(messageKey, (int)messageKey, args)));
    }
}