namespace Backend.Libs.Messaging.Options;

public class RabbitMqOptions
{
    public const string Name = nameof(RabbitMqOptions);

    public required string Password { get; init; } = "root";
    public required string Username { get; init; } = "root";
    public required string Hostname { get; init; } = "localhost";
    public required short Port { get; init; } = 5672;
   
}