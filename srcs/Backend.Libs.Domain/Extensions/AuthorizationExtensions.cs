using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.Domain.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddPermissionBasedAuthorization<TPermission, TRole>(
        this IServiceCollection serviceCollection)
        where TPermission : Enum
        where TRole: Enum

    {
        serviceCollection.AddAuthorization(options =>
        {
            Type permissionEnumType = typeof(TPermission);
            Type roleEnumType = typeof(TRole);
            
            string[] permissionEnumNames = Enum.GetNames(permissionEnumType);
            List<string> roleEnumNames = Enum.GetNames(roleEnumType).ToList();
            
            foreach (string permissionName in permissionEnumNames)
            {
                MemberInfo permissionMemberInfo = permissionEnumType.GetMember(permissionName)[0];

                DisplayAttribute? attr = permissionMemberInfo.GetCustomAttribute<DisplayAttribute>();

                if (attr == null)
                {
                    throw new Exception($"Permission {permissionName} does not have a Display attribute");
                }
                
                if(string.IsNullOrEmpty(attr.GroupName) || !roleEnumNames.Contains(attr.GroupName))
                {
                    throw new Exception($"Permission {permissionName} does not have a valid GroupName");
                }
                
                options.AddPolicy(permissionName, policy =>
                {
                    policy.RequireRole(attr.GroupName);
                });
            }
        });
        return serviceCollection;
    }
}