using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class CourseTagConfiguration : IEntityTypeConfiguration<CourseTag>
    {
        public void Configure(EntityTypeBuilder<CourseTag> builder)
        {
            builder.HasIndex(ct => new { ct.CourseId, ct.TagId }).IsUnique();

            builder.HasOne(ct => ct.Course)
                .WithMany(c => c.Tags)
                .HasForeignKey(ct => ct.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ct => ct.Tag)
                .WithMany()
                .HasForeignKey(ct => ct.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
