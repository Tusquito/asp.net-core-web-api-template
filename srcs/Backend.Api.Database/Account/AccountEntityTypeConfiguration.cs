using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Api.Database.Account;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.TestEntity)
            .WithOne(x => x.AccountEntity)
            .HasForeignKey<TestEntity>(x => x.AccountId);
    }
}