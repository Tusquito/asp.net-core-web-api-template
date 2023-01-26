using Backend.Libs.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Libs.Domain.Attributes;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(PermissionType permissionType)
    {
        Policy = permissionType.ToString();
    }
}