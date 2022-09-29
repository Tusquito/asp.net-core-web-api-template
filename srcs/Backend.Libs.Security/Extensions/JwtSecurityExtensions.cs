using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Libs.Cryptography.Extensions;
using Backend.Libs.Database.Account;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Libs.Security.Extensions;

public static class JwtSecurityExtensions
{
    private static readonly int JwtExpiryTime = Convert.ToInt32(Environment.GetEnvironmentVariable("JWT_EXPIRY_TIME_IN_HOURS") ?? "1");
    private static readonly byte[] JwtSignatureKey = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SIGNATURE_KEY")?.ToSha512() ?? "123456789".ToSha512());
    private static readonly JwtSecurityTokenHandler TokenHandler = new();
    
    public static string GenerateJwtToken(this AccountDTO account)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "backend",
            Audience = "backend",
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.AuthorityType.ToJwtRole())
            }),
            Expires = DateTime.UtcNow.AddHours(JwtExpiryTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(JwtSignatureKey), SecurityAlgorithms.HmacSha256Signature)
        };
        return TokenHandler.WriteToken(TokenHandler.CreateToken(tokenDescriptor));
    }

    public static JwtSecurityToken ValidateToken(this string token)
    {
        TokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(JwtSignatureKey),
            ValidateIssuer = true,
            ValidAlgorithms = new []{SecurityAlgorithms.HmacSha256Signature},
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        return (JwtSecurityToken)validatedToken;
    }
}