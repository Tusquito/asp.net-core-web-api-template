using System.Text.Json.Serialization;
using Backend.Libs.Database.Generic;

namespace Backend.Libs.Database.Account;

public class AccountDTO : IUuidDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
    [JsonIgnore]
    public string Ip { get; set; } = string.Empty;
    public AuthorityType AuthorityType { get; set; }
    [JsonIgnore]
    public string PasswordSalt { get; set; } = string.Empty;
}