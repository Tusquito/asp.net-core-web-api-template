using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Libs.Database.Account;
using Backend.Libs.Database.Generic;
using Backend.Plugins.Database.Context;

namespace Backend.Plugins.Database.Entities;

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
    public AuthorityType AuthorityType { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}