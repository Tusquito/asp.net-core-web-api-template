using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Backend.Libs.Security.Extensions;

public static class HttpAccessorExtensions
{
    internal static Guid GetAccountIdOrThrow(this IHttpContextAccessor accessor)
    {
        Claim? tmp = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (tmp == null)
        {
            throw new ArgumentException("Can not retrieve accountId from jwt claims");
        }

        return !Guid.TryParse(tmp.Value, out Guid id) ? Guid.Empty : id;
    }

    internal static string GetJwtOrThrow(this IHttpContextAccessor accessor)
    {
        string? authHeader = accessor.HttpContext?.Request.Headers.Authorization;

        if (string.IsNullOrEmpty(authHeader))
        {
            throw new ArgumentException("Can not retrieve authorization header");
        }

        string[] authHeaderArray = authHeader.Split(' ');

        if (authHeaderArray.Length != 2)
        {
            throw new ArgumentException("Can not split authorization header");
        }


        if (string.IsNullOrWhiteSpace(authHeaderArray[1]))
        {
            throw new ArgumentException("Can not retrieve jwt token from authorization header");
        }

        return authHeaderArray[1];
    }
}