namespace Backend.Libs.Models.Authentication;

public class LoginRequestForm
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}