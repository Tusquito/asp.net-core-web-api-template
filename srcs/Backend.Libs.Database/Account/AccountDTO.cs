using System.Text.Json.Serialization;
using Backend.Libs.Database.Generic;
using ProtoBuf;

namespace Backend.Libs.Database.Account;
[ProtoContract]
public class AccountDto : IUuidDto
{
    [ProtoMember(1)]
    public Guid Id { get; set; }
    [ProtoMember(2)]
    public string Username { get; set; } = string.Empty;
    [ProtoMember(3)]
    public string Email { get; set; } = string.Empty;
    [ProtoMember(4)]
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
    [ProtoMember(5)]
    [JsonIgnore]
    public string Ip { get; set; } = string.Empty;
    [ProtoMember(6)] 
    public List<RoleType> Roles { get; set; } = new();
    [JsonIgnore]
    [ProtoMember(7)]
    public string PasswordSalt { get; set; } = string.Empty;
}