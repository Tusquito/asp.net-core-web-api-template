namespace Backend.Libs.Models.Account;

public class RegisterRequestForm
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string PasswordConfirmation { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}