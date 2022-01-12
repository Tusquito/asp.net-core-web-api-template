using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Domain.Account;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Libs.Security.Extensions;

public static class JwtSecurityExtensions
{
    private static readonly int JwtExpiryTime = Convert.ToInt32(Environment.GetEnvironmentVariable("JWT_EXPIRY_TIME_IN_HOURS") ?? "1");
    private static readonly byte[] JwtSignatureKey = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SIGNATURE_KEY")?.ToSha512() ?? "123456789".ToSha512());
    private static readonly JwtSecurityTokenHandler TokenHandler = new();
    
    public static string GenerateJwtToken(this AccountDto account)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, ((int)account.AuthorityType).ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(JwtExpiryTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(JwtSignatureKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = TokenHandler.CreateToken(tokenDescriptor);
        return TokenHandler.WriteToken(token);
    }

    public static JwtSecurityToken? ValidateToken(this string token)
    {
        try
        {
            TokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(JwtSignatureKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
        catch
        {
            return null;
        }
    }
}