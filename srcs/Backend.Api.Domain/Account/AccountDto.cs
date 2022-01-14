using System;
using System.Text.Json.Serialization;
using Backend.Domain.Enums;

namespace Backend.Domain.Account;

public class AccountDto : IUuidDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    [JsonIgnore]
    public string Ip { get; set; }
    public AuthorityType AuthorityType { get; set; }
    [JsonIgnore]
    public string PasswordSalt { get; set; }
    public Guid TestId { get; set; }
}