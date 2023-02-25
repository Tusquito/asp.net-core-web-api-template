namespace Backend.Libs.Models.Authentication;

public class TokenModel
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required string TokenType { get; init; }
}