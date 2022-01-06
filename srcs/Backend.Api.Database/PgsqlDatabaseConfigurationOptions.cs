namespace Backend.Api.Database
{
    public class PgsqlDatabaseConfigurationOptions
    {
        public static string Name = nameof(PgsqlDatabaseConfigurationOptions);
        public string Ip { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Database { get; init; }
        public ushort Port { get; init; }

        public override string ToString() => $"Host={Ip};Port={Port.ToString()};Database={Database};Username={Username};Password={Password}";
    }
}