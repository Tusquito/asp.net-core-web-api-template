namespace Backend.Libs.Infrastructure.Options;

public class RabbitMqOptions
{
    public required string Password { get; init; } = "root";
    public required string Username { get; init; } = "root";
    public required string Hostname { get; init; } = "localhost";
    public required ushort Port { get; init; } = 5672;
   
}