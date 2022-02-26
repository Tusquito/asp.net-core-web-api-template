namespace Backend.Libs.Models.Login;

public class LoginResponseModel
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public string TokenType = "Bearer";
}