using System.Security.Cryptography;
using System.Text;
using Backend.Libs.Persistence.Enums;

namespace Backend.Libs.Domain.Extensions;

public static class StringHashingExtensions
{
    public static string ToSha512(this string str)
    {
        using SHA512 hash = SHA512.Create();
        return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(str)).Select(item => item.ToString("x2")));
    }
    
    public static string ToJwtRole(this RoleType roleType)
    {
        return ((int)roleType).ToString();
    }
}