using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.Property(s => s.Title).IsRequired().HasMaxLength(150);
            builder.HasIndex(s => new { s.CourseId, s.Order }).IsUnique();

            builder.OwnsOne(s => s.TotalDuration, b =>
            {
                b.Property(p => p.Minutes).HasColumnName("SectionTotalDurationMinutes");
            });
            builder.Navigation(s => s.TotalDuration).IsRequired(false);

            builder.HasOne(s => s.Course)
                .WithMany(c => c.Sections)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Lessons)
                .WithOne(l => l.Section)
                .HasForeignKey(l => l.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Quiz)
                .WithOne(q => q.Section)
                .HasForeignKey<Quiz>(q => q.SectionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
