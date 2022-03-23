#nullable enable
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Backend.Libs.Domain.Results;

[DefaultStatusCode(DefaultStatusCode)]
public class InternalServerErrorObjectResult : ObjectResult
{
    private const int DefaultStatusCode = StatusCodes.Status500InternalServerError;
    public InternalServerErrorObjectResult(object? value) : base(value)
    {
        StatusCode = DefaultStatusCode;
    }
}