namespace Backend.Libs.Models.Authentication;

public class LoginRequest
{
    public string Login { get; init; }
    public string Password { get; init; }
}