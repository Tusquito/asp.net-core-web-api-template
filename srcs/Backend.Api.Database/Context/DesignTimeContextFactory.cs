using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend.Api.Database.Context;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<BackendDbContext>
{
    public BackendDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BackendDbContext>();
        var config = PgsqlDatabaseConfiguration.FromEnv();
        optionsBuilder.UseNpgsql(config.ToString());
        return new BackendDbContext(optionsBuilder.Options, config);
    }
}