namespace Backend.Libs.Redis;

public class RedisConfiguration
{
    private RedisConfiguration(string host, int port, string password)
    {
        Host = host;
        Port = port;
        Password = password;
    }

    public string Host { get; }
    public int Port { get; }
    public string Password { get; }

    public static RedisConfiguration FromEnv() =>
        new(
            Environment.GetEnvironmentVariable("REDIS_IP") ?? "localhost",
            Convert.ToInt32(Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379"),
            Environment.GetEnvironmentVariable("REDIS_PASSWORD")!
        );
    
}