using AuthPermissions.AspNetCore;
using Backend.Libs.Domain.Enums;

namespace Backend.Api.Authentication.Endpoints;

[HasPermission(PermissionsType.UserLogout)]
public class LogoutEndpoint
{
    
}