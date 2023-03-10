namespace Backend.Libs.Domain.Forms.Account;

public class RegisterRequestForm
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string PasswordConfirmation { get; init; }
    public required string Email { get; init; }
}