using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Backend.Libs.Domain.HttpResults;

[DefaultStatusCode(DefaultStatusCode)]
public class CreatedObjectResult : ObjectResult
{
    private const int DefaultStatusCode = StatusCodes.Status201Created;
    public CreatedObjectResult(object? value) : base(value)
    {
        StatusCode = DefaultStatusCode;
    }
}