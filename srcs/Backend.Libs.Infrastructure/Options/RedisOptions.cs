namespace Backend.Libs.Infrastructure.Options;

public class RedisOptions
{
    public required string Host { get; init; } = "localhost";

    public required ushort Port { get; init; } = 6379;
    public string? Password { get; init; } = null;

    public override string ToString()
    {
        return $"{Host}:{Port}";
    }
}