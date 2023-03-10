using Backend.Libs.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Libs.Persistence.Extensions;

public static class MigrationExtensions
{
    public static async Task TryMigrateEfCoreDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IDbContextFactory<BackendDbContext> dbContextFactory =
            scope.ServiceProvider.GetRequiredService<IDbContextFactory<BackendDbContext>>();
        await using BackendDbContext context = await dbContextFactory.CreateDbContextAsync();
        await context.Database.MigrateAsync();
    }
}