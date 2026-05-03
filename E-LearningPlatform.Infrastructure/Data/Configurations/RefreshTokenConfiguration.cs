using Microsoft.EntityFrameworkCore;

using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.TokenHash).IsRequired().HasMaxLength(256);
            builder.HasIndex(x => x.TokenHash).IsUnique();
            builder.Property(x => x.ExpiresAtUtc).IsRequired();
        }
    }
}
