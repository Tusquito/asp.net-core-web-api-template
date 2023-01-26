using Backend.Libs.Domain.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Backend.Libs.Domain.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequesterCultureMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequesterCultureMiddleware>();
    }
    
    public static IApplicationBuilder UseRequesterIpMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequesterIpMiddleware>();
    }
}