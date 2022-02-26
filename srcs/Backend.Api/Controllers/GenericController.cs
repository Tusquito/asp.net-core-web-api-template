using Backend.Libs.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

internal record GenericResponse(int Status, object Data, GenericResponseMessage Message);
internal record GenericResponseMessage(ResponseMessageKey Key, string[] Params);
    
public class GenericController : Controller
{
    [NonAction]
    public IActionResult OkResponse(object data = null, ResponseMessageKey messageKey = ResponseMessageKey.DEFAULT_SUCCESS, string[] @params = null)
    {
        return Json(new GenericResponse(StatusCodes.Status200OK, data, new GenericResponseMessage(messageKey, @params)));
    }
        
    [NonAction]
    public IActionResult BadRequestResponse(ResponseMessageKey messageKey = ResponseMessageKey.DEFAULT_BAD_REQUEST, string[] @params = null)
    {
        return Json(new GenericResponse(StatusCodes.Status400BadRequest, null, new GenericResponseMessage(messageKey, @params)));
    }
        
    [NonAction]
    public IActionResult NotFoundResponse(ResponseMessageKey messageKey = ResponseMessageKey.DEFAULT_NOT_FOUND, string[] @params = null)
    {
        return Json(new GenericResponse(StatusCodes.Status404NotFound, null, new GenericResponseMessage(messageKey, @params)));
    }
        
    [NonAction]
    public IActionResult InternalServerErrorResponse(ResponseMessageKey messageKey = ResponseMessageKey.DEFAULT_INTERNAL_SERVER_ERROR, string[] @params = null)
    {
        return Json(new GenericResponse(StatusCodes.Status500InternalServerError, null, new GenericResponseMessage(messageKey, @params)));
    }
}