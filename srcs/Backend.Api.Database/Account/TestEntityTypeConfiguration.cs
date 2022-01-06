using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Api.Database.Account
{
    public class TestEntityTypeConfiguration : IEntityTypeConfiguration<TestEntity>
    {
        public void Configure(EntityTypeBuilder<TestEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.AccountEntity)
                .WithOne(x => x.TestEntity)
                .HasForeignKey<AccountEntity>(x => x.TestId);
        }
    }
}