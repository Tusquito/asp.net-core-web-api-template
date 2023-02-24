﻿using Backend.Libs.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend.Plugins.Database.Context;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<BackendDbContext>
{
    public BackendDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<BackendDbContext> optionsBuilder = new DbContextOptionsBuilder<BackendDbContext>();
        optionsBuilder.UseNpgsql(PgsqlDatabaseConfiguration.FromEnv().ToString());
        return new BackendDbContext(optionsBuilder.Options);
    }
}