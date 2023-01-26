﻿using System.ComponentModel.DataAnnotations;
using Backend.Libs.Database.Account;

namespace Backend.Libs.Domain.Enums;

public enum PermissionType : ushort
{
    [Display(GroupName = nameof(RoleType.User), Name = nameof(UserLogout))]
    UserLogout = 1001,
    
    [Display(GroupName = nameof(RoleType.Tester), Name = nameof(TestGetAccount))]
    TestGetAccount = 2001,
    [Display(GroupName = nameof(RoleType.Tester), Name = nameof(TestCreateAccount))]
    TestCreateAccount = 2002,
    
    [Display(GroupName = nameof(RoleType.Root), Name = nameof(AccessAll))]
    AccessAll = ushort.MaxValue,
}