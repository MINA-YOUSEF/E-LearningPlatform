using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.Property(q => q.Title).IsRequired().HasMaxLength(200);
            builder.HasOne(q => q.Section)
                .WithOne(s => s.Quiz)
                .HasForeignKey<Quiz>(q => q.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.Questions)
                .WithOne(qn => qn.Quiz)
                .HasForeignKey(qn => qn.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
