using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend.Libs.Persistence.Context;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<BackendDbContext>
{
    public BackendDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<BackendDbContext> optionsBuilder = new DbContextOptionsBuilder<BackendDbContext>();
        optionsBuilder.UseNpgsql(PgsqlDatabaseConfiguration.FromEnv().ToString());
        return new BackendDbContext(optionsBuilder.Options);
    }
}