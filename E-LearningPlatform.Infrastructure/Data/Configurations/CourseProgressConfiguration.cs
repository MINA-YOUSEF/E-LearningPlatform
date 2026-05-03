using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class CourseProgressConfiguration : IEntityTypeConfiguration<CourseProgress>
    {
        public void Configure(EntityTypeBuilder<CourseProgress> builder)
        {
            builder.HasIndex(p => new { p.CourseId, p.UserId }).IsUnique();
            builder.Property(p => p.ProgressPercent).HasPrecision(5, 2);

            builder.HasOne(p => p.Course)
                .WithMany()
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
