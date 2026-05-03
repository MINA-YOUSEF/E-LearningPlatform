using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.Property(l => l.Title).IsRequired().HasMaxLength(200);
            builder.Property(l => l.ContentType).IsRequired();
            builder.HasIndex(l => new { l.SectionId, l.Order }).IsUnique();

            builder.OwnsOne(l => l.Duration, b =>
            {
                b.Property(p => p.Minutes).HasColumnName("DurationMinutes").IsRequired();
            });

            builder.HasOne(l => l.Section)
                .WithMany(s => s.Lessons)
                .HasForeignKey(l => l.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.MediaFiles)
                .WithOne(m => m.Lesson)
                .HasForeignKey(m => m.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.Comments)
                .WithOne(c => c.Lesson)
                .HasForeignKey(c => c.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.Progresses)
                .WithOne(p => p.Lesson)
                .HasForeignKey(p => p.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
