namespace Backend.Libs.Models.Authentication;

public class LoginRequestForm
{
    public string Login { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}