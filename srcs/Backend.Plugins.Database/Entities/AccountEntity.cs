﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Libs.Database.Account;
using Backend.Libs.Database.Generic;
using Backend.Plugins.Database.Context;

namespace Backend.Plugins.Database.Entities;

[Table("account", Schema = DatabaseSchemas.Accounts)]
public class AccountEntity : IUuidEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Encrypted] 
    public required string Username { get; set; }
    [Encrypted]
    public required string Email { get; set; }

    [Encrypted] 
    public required string Password { get; set; }

    [Encrypted]
    public required string PasswordSalt { get; set; } = string.Empty;

    [Encrypted] 
    public string Ip { get; set; } = string.Empty;

    public List<RoleType> Roles { get; set; } = new();
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}