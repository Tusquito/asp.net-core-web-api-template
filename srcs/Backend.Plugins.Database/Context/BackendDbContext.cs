using Backend.Libs.Database;
using Backend.Libs.Database.Generic;
using Backend.Plugins.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;

namespace Backend.Plugins.Database.Context;

public class BackendDbContext : DbContext
{
    private readonly IEncryptionProvider _encryptionProvider;
    private readonly PgsqlDatabaseConfiguration _dbConfig = PgsqlDatabaseConfiguration.FromEnv();
    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
    {
        byte[] encryptionKey = Convert.FromBase64String(_dbConfig.EncryptionKey ?? throw new ArgumentException("PGSQL_DATABASE_ENCRYPTION_KEY env variable missing"));
        byte[] encryptionIv = Convert.FromBase64String(_dbConfig.EncryptionIv ?? throw new ArgumentException("PGSQL_DATABASE_ENCRYPTION_IV env variable missing"));
        _encryptionProvider = new AesProvider(encryptionKey, encryptionIv);
    }

    public DbSet<AccountEntity> Accounts { get; set; } = null!;

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSaving()
    {
        IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
        DateTime utcNow = DateTime.UtcNow;

        foreach (EntityEntry entry in entries)
        {
            if (entry.Entity is IEntity trackable)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        trackable.UpdatedOn = utcNow;
                        entry.Property(nameof(trackable.CreatedOn)).IsModified = false;
                        break;
                    case EntityState.Added:
                        trackable.CreatedOn = utcNow;
                        trackable.UpdatedOn = utcNow;
                        break;
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                    case EntityState.Deleted:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseEncryption(_encryptionProvider);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BackendDbContext).Assembly);
    }
}