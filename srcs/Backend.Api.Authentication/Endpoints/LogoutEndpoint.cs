using Backend.Libs.Domain.Attributes;
using Backend.Libs.Domain.Enums;

namespace Backend.Api.Authentication.Endpoints;

[HasPermission(PermissionType.UserLogout)]
public class LogoutEndpoint
{
    
}