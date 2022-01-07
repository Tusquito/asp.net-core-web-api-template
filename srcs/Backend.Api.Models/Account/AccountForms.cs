namespace Backend.Api.Models.Account;

public class AccountRegisterForm
{
    public string Username { get; init; }
    public string Password { get; init; }
    public string PasswordConfirmation { get; init; }
    public string Email { get; init; }
}