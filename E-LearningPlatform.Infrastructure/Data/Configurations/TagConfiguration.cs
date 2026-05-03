using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Slug).IsRequired().HasMaxLength(150);
            builder.HasIndex(t => t.Slug).IsUnique();
        }
    }
}
