using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Api.Database.Context;
using Backend.Api.Database.Generic;
using Backend.Domain.Enums;

namespace Backend.Api.Database.Account;

[Table("account", Schema = DatabaseSchemas.ACCOUNTS)]
public class AccountEntity : IUuidEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Encrypted]
    public string Username { get; set; }
    [Encrypted]
    public string Email { get; set; }
    [Encrypted]
    public string Password { get; set; }
    [Encrypted]
    public string PasswordSalt { get; set; }
    [Encrypted]
    public string Ip { get; set; }
    public Guid TestId { get; set; }
    public AuthorityType AuthorityType { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public virtual TestEntity TestEntity { get; set; }
        
}