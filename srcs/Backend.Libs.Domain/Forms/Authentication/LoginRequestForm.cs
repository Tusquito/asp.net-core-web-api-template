namespace Backend.Libs.Domain.Forms.Authentication;

public class LoginRequestForm
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}