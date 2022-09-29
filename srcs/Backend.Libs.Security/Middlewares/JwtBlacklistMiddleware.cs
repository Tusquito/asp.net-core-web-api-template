using Backend.Libs.Security.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Libs.Security.Middlewares;

public class JwtBlacklistMiddleware
{
    private readonly RequestDelegate _next;
    //private readonly IRedisJwtSessionBlacklistService _sessionBlacklistService;
    private readonly ILogger<JwtBlacklistMiddleware> _logger;
    private readonly IHttpContextAccessor _contextAccessor;
    //private readonly IJwtTokenFactory _jwtTokenFactory;
    public JwtBlacklistMiddleware(RequestDelegate next, ILogger<JwtBlacklistMiddleware> logger, IHttpContextAccessor contextAccessor)
    {
        _next = next;
        _logger = logger;
        _contextAccessor = contextAccessor;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _contextAccessor.GetJwtOrThrow().ValidateToken();
            await _next(context);
            /*if (await _sessionBlacklistService.IsSessionBlacklistedAsync(sessionId))
            {
                _logger.LogWarning("[{Scope}] Session ({SessionId}) is still used but blacklisted",
                    nameof(JwtBlacklistMiddleware), sessionId);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }*/
        }
        catch (SecurityTokenInvalidLifetimeException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (ArgumentException)
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(JwtBlacklistMiddleware));
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}

public static class JwtSessionBlacklistMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtBlacklistMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<JwtBlacklistMiddleware>();
}