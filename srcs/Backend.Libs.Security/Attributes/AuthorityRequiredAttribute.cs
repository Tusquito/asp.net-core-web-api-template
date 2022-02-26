using Backend.Libs.Cryptography.Extensions;
using Backend.Libs.Database.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Libs.Security.Attributes;


public class AuthorityRequiredAttribute : AuthorizeAttribute
{
    public AuthorityRequiredAttribute(AuthorityType authorityType)
    {
        IEnumerable<AuthorityType> enums = Enum.GetValues(typeof(AuthorityType)).Cast<AuthorityType>().Where(s => s >= authorityType);
        Roles = string.Join(",", enums.Select(s => s.ToJwtRole()));
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}