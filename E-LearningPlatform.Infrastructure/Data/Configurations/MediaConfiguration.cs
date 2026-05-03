using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.Property(m => m.PublicId).IsRequired().HasMaxLength(200);
            builder.Property(m => m.Url).IsRequired();
            builder.Property(m => m.MimeType).HasMaxLength(150);
            builder.HasIndex(m => m.LessonId);

            builder.HasOne(m => m.Lesson)
                .WithMany(l => l.MediaFiles)
                .HasForeignKey(m => m.LessonId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
