using System;
using System.Text.Json.Serialization;
using Backend.Domain.Enums;

namespace Backend.Api.Models.Account;

public class AccountModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AuthorityType AuthorityType { get; set; }//TODO Fix not mapped property
}