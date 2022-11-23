using System;
using Backend.Libs.Database.Account;

namespace Backend.Libs.Models.Account;

public class AccountRequestQuery
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Ip { get; set; }
    public RoleType RoleType { get; set; } 
}