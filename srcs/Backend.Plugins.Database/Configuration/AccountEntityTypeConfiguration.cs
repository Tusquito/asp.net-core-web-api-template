using Backend.Plugins.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Plugins.Database.Configuration;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.Username, x.Email })
            .IsUnique();
}
}