namespace Backend.Libs.Database;

public class PgsqlDatabaseConfiguration
{
    public string Ip { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string Database { get; init; }
    public ushort Port { get; init; }
    public string EncryptionKey{ get; init; }
    public string EncryptionIv{ get; init; }


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