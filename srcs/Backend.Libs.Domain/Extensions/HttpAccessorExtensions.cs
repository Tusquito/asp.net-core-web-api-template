using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Backend.Libs.Domain.Extensions;

public static class HttpAccessorExtensions
{
    public static Guid GetAccountId(this IHttpContextAccessor accessor)
    {
        Claim claim = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {
            return Guid.Empty;
        }

        return !Guid.TryParse(claim.Value, out Guid id) ? Guid.Empty : id;
    }
    
    public static string GetRequestIp(this IHttpContextAccessor accessor, bool tryUseXForwardHeader = true)
    {
        string ip = null;
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

        return ip;
    }

    private static T GetHeaderValueAs<T>(this IHttpContextAccessor accessor, string headerName)
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

    private static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
    {
        if (string.IsNullOrWhiteSpace(csvList))
        {
            return nullOrWhitespaceInputReturnsNull ? null : new List<string>();
        }

        return csvList
            .TrimEnd(',')
            .Split(',')
            .AsEnumerable()
            .Select(s => s.Trim())
            .ToList();
    }
}