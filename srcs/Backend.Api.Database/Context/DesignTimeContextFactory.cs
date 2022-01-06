using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend.Api.Database.Context
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<WebContext>
    {
        public WebContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebContext>();
            optionsBuilder.UseNpgsql(new DatabaseConfiguration().ToString());
            return new WebContext(optionsBuilder.Options);
        }
    }
}