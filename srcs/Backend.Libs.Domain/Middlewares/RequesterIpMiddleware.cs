using Backend.Libs.Domain.Extensions;
using Microsoft.AspNetCore.Http;

namespace Backend.Libs.Domain.Middlewares;

public class RequesterIpMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequesterIpMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _httpContextAccessor.GetRequestIpOrThrow();
        }
        catch (ArgumentException)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }

        await _next(context);
    }
}