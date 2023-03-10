using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Backend.Libs.Domain.Extensions;

public static class HttpAccessorExtensions
{
    public static string GetRequestIpOrThrow(this IHttpContextAccessor accessor, bool tryUseXForwardHeader = true)
    {
        string? ip = null;
        if (tryUseXForwardHeader)
        {
            ip = accessor.GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();
        }
        
        if (string.IsNullOrWhiteSpace(ip) && accessor.HttpContext?.Connection.RemoteIpAddress != null)
        {
            ip = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        if (string.IsNullOrWhiteSpace(ip))
        {
            ip = accessor.GetHeaderValueAs<string>("REMOTE_ADDR");
        }

        return ip ?? throw new ArgumentException("Can not retrieve request ip from headers.");
    }

    private static T? GetHeaderValueAs<T>(this IHttpContextAccessor accessor, string headerName)
    {
        if (accessor.HttpContext == null || accessor.HttpContext.Request.Headers.TryGetValue(headerName, out StringValues values) == false)
        {
            return default;
        }

        string rawValues = values.ToString(); // writes out as Csv when there are multiple.

        if (!string.IsNullOrWhiteSpace(rawValues))
        {
            return (T)Convert.ChangeType(values.ToString(), typeof(T));
        }

        return default;
    }

    private static List<string> SplitCsv(this string? csvList)
    {
        if (string.IsNullOrWhiteSpace(csvList))
        {
            return new List<string>();
        }

        return csvList
            .TrimEnd(',')
            .Split(',')
            .AsEnumerable()
            .Select(s => s.Trim())
            .ToList();
    }
    
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