namespace Backend.Libs.Persistence;

public class PgsqlDatabaseConfiguration
{
    public required string Ip { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Database { get; init; }
    public required ushort Port { get; init; }
    public required string EncryptionKey{ get; init; }
    public required string EncryptionIv{ get; init; }


    public static PgsqlDatabaseConfiguration FromEnv()
    {
        return new PgsqlDatabaseConfiguration
        {
            Ip = Environment.GetEnvironmentVariable("PGSQL_DATABASE_IP") ?? "localhost",
            Username = Environment.GetEnvironmentVariable("PGSQL_DATABASE_USERNAME") ?? "postgres",
            Password = Environment.GetEnvironmentVariable("PGSQL_DATABASE_PASSWORD") ?? "postgres",
            Database = Environment.GetEnvironmentVariable("PGSQL_DATABASE_NAME") ?? "postgres",
            Port = ushort.Parse(Environment.GetEnvironmentVariable("PGSQL_DATABASE_PORT") ?? "5432"),
            EncryptionKey = Environment.GetEnvironmentVariable("PGSQL_DATABASE_ENCRYPTION_KEY") ?? "AAECAwQFBgcICQoLDA0ODw==",
            EncryptionIv = Environment.GetEnvironmentVariable("PGSQL_DATABASE_ENCRYPTION_IV") ?? "AAECAwQFBgcICQoLDA0ODw=="
        };
    }

    public override string ToString() => $"Host={Ip};Port={Port.ToString()};Database={Database};Username={Username};Password={Password}";
}