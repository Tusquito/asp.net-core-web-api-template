namespace Backend.Libs.Messaging.Options;

public class RabbitMqOptions
{
    public const string Name = nameof(RabbitMqOptions);

    public string Password { get; init; } = "bitnami";
    public string Username { get; init; } = "user";
    public string Hostname { get; init; } = "localhost";
    public short Port { get; init; } = 5672;
   
}