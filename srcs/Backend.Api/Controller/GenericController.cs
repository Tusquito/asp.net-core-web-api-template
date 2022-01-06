using System.Net;
using Backend.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controller
{
    internal record GenericResponseData(int Status, object Data, GenericResponseDataMessage MessageData);
    internal record GenericResponseDataMessage(ResponseMessageKey MessageKey, string[] Params);
    
    [ApiExplorerSettings(IgnoreApi = true)]
    public class GenericController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult OkResponse(object data = null, ResponseMessageKey messageKey = ResponseMessageKey.DEFAULT_SUCCESS, string[] @params = null)
        {
            return Json(new GenericResponseData((int)HttpStatusCode.OK, data, new GenericResponseDataMessage(messageKey, @params)));
        }
        
        public IActionResult BadRequestResponse(ResponseMessageKey messageKey = ResponseMessageKey.DEFAULT_BAD_REQUEST, string[] @params = null)
        {
            return Json(new GenericResponseData((int)HttpStatusCode.BadRequest, null, new GenericResponseDataMessage(messageKey, @params)));
        }
        
        public IActionResult NotFoundResponse(ResponseMessageKey messageKey = ResponseMessageKey.DEFAULT_NOT_FOUND, string[] @params = null)
        {
            return Json(new GenericResponseData((int)HttpStatusCode.NotFound, null, new GenericResponseDataMessage(messageKey, @params)));
        }
        
        public IActionResult InternalServerErrorResponse(ResponseMessageKey messageKey = ResponseMessageKey.DEFAULT_INTERNAL_SERVER_ERROR, string[] @params = null)
        {
            return Json(new GenericResponseData((int)HttpStatusCode.InternalServerError, null, new GenericResponseDataMessage(messageKey, @params)));
        }
    }
}