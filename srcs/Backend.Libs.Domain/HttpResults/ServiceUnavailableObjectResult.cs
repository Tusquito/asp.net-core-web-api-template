using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Backend.Libs.Domain.HttpResults;

[DefaultStatusCode(DefaultStatusCode)]
public class ServiceUnavailableObjectResult : ObjectResult
{
    private const int DefaultStatusCode = StatusCodes.Status503ServiceUnavailable;
    public ServiceUnavailableObjectResult(object? value) : base(value)
    {
        StatusCode = DefaultStatusCode;
    }
}