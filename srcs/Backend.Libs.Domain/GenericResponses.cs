using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Libs.Domain;

internal record GenericResponse(int Status, object Data, GenericResponseMessage Message);
internal record GenericResponseMessage(ResponseMessageKey Key, int Code, string[] Params);
    
public static class GenericResponses
{
    public static OkObjectResult Ok(object data = null, ResponseMessageKey messageKey = ResponseMessageKey.SUCCESS, string[] @params = null)
    {
        return new OkObjectResult(new GenericResponse(StatusCodes.Status200OK, data, new GenericResponseMessage(messageKey, (int)messageKey, @params)));
    }
    
    public static BadRequestObjectResult BadRequest(ResponseMessageKey messageKey = ResponseMessageKey.BAD_REQUEST, string[] @params = null)
    {
        return new BadRequestObjectResult(new GenericResponse(StatusCodes.Status400BadRequest, null, new GenericResponseMessage(messageKey, (int)messageKey, @params)));
    }
    
    public static NotFoundObjectResult NotFoundResponse(ResponseMessageKey messageKey = ResponseMessageKey.NOT_FOUND, string[] @params = null)
    {
        return new NotFoundObjectResult(new GenericResponse(StatusCodes.Status404NotFound, null, new GenericResponseMessage(messageKey, (int)messageKey, @params)));
    }
    
    public static UnauthorizedObjectResult UnauthorizedResponse(ResponseMessageKey messageKey = ResponseMessageKey.UNAUTHORIZED, string[] @params = null)
    {
        return new UnauthorizedObjectResult(new GenericResponse(StatusCodes.Status401Unauthorized, null, new GenericResponseMessage(messageKey, (int)messageKey, @params)));
    }
    
    public static InternalServerErrorObjectResult InternalServerError(ResponseMessageKey messageKey = ResponseMessageKey.INTERNAL_SERVER_ERROR, string[] @params = null)
    {
        return new InternalServerErrorObjectResult(new GenericResponse(StatusCodes.Status500InternalServerError, null, new GenericResponseMessage(messageKey, (int)messageKey, @params)));
    }
}