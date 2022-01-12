using System.Security.Cryptography;
using System.Text;
using Backend.Domain.Enums;

namespace Backend.Libs.Security.Extensions;

public static class StringHashingExtensions
{
    public static string ToSha512(this string str)
    {
        using var hash = SHA512.Create();
        return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(str)).Select(item => item.ToString("x2")));
    }
    
    public static string ToJwtRole(this AuthorityType authorityType)
    {
        return ((int)authorityType).ToString();
    }
}