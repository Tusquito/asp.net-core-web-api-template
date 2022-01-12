using System;
using System.Text.Json.Serialization;
using Backend.Domain.Enums;

namespace Backend.Domain.Account;

public class AccountDto : IUuidDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Ip { get; set; }
    public AuthorityType AuthorityType { get; set; }
    public string PasswordSalt { get; set; }
    public Guid TestId { get; set; }
}