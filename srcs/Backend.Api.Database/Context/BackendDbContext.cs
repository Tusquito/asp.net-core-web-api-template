﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Backend.Api.Database.Account;
using Backend.Api.Database.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;

namespace Backend.Api.Database.Context
{
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
        
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<TestEntity> Tests { get; set; }

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
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
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
            modelBuilder.ApplyConfiguration(new AccountEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TestEntityTypeConfiguration());
        }
    }
}