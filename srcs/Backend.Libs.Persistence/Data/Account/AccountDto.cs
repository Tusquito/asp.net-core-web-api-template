using System.Text.Json.Serialization;
using Backend.Libs.Persistence.Data.Abstractions;
using Backend.Libs.Persistence.Enums;
using ProtoBuf;

namespace Backend.Libs.Persistence.Data.Account;
[ProtoContract]
public class AccountDto : IUuidDto
{
    [ProtoMember(1)]
    public Guid Id { get; set; }
    [ProtoMember(2)]
    public required string Username { get; set; }
    [ProtoMember(3)]
    public required string Email { get; set; }
    [ProtoMember(4)]
    [JsonIgnore]
    public required string Password { get; set; }
    [ProtoMember(5)]
    [JsonIgnore]
    public required string Ip { get; set; }
    [ProtoMember(6)] 
    public required List<RoleType> Roles { get; set; }
    [JsonIgnore]
    [ProtoMember(7)]
    public required string PasswordSalt { get; set; }
}