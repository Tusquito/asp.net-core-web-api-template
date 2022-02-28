namespace Backend.Libs.Models.Account;

public class RegisterRequest
{
    public string Username { get; init; }
    public string Password { get; init; }
    public string PasswordConfirmation { get; init; }
    public string Email { get; init; }
}