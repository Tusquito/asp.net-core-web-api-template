using Backend.Libs.Persistence.Enums;

namespace Backend.Libs.Domain.Queries.Account;

public class AccountRequestQuery
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public RoleType RoleType { get; set; }
}