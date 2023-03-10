using Backend.Libs.Domain.Extensions;
using Backend.Libs.Persistence.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Libs.Domain.Attributes;


public class AuthorityRequiredAttribute : AuthorizeAttribute
{
    public AuthorityRequiredAttribute(RoleType roleType)
    {
        IEnumerable<RoleType> enums = Enum.GetValues(typeof(RoleType)).Cast<RoleType>().Where(s => s >= roleType);
        Roles = string.Join(",", enums.Select(s => s.ToJwtRole()));
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}