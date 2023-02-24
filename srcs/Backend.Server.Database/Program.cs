using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Libs.gRPC.Enums;
using Backend.Plugins.Database.Context;
using Backend.Plugins.Database.Mapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend.Server.Database;

public class Program
{
    public static async Task Main(string[] args)
    {
        MapsterMapperRules.InitMappingRules();
        IHost web = CreateHostBuilder(args).Build();

        ILogger<Program> logger = web.Services.GetRequiredService<ILogger<Program>>();
        
        IDbContextFactory<BackendDbContext> tmp = web.Services.GetRequiredService<IDbContextFactory<BackendDbContext>>();
        await using BackendDbContext context = await tmp.CreateDbContextAsync();
        try
        {
            IEnumerable<string> pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            IEnumerable<string> appliedMigrations = await context.Database.GetAppliedMigrationsAsync();

            await context.Database.MigrateAsync();
            IEnumerable<string> pendingMigrationsAfter = await context.Database.GetPendingMigrationsAsync();
            logger.LogWarning("Database migration executed: {pendingMigrationsCountBefore} => {pendingMigrationsCountAfter} | totalMigrations: {appliedMigrationsCount}",
                pendingMigrations.Count().ToString(), pendingMigrationsAfter.Count().ToString(), appliedMigrations.Count().ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Migration failed");
            return;
        }

        logger.LogWarning("Starting...");
        await web.StartAsync();
        
        logger.LogWarning("Running!");
        await web.WaitForShutdownAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        IHostBuilder host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(s =>
                {
                    s.ConfigureEndpointDefaults(co =>
                    {
                        co.Protocols = HttpProtocols.Http2;
                    });
                });
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls($"http://*:{(short)GrpcServiceType.DatabaseServerPort}");
            });
        return host;
    }
}