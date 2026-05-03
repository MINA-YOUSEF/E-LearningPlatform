using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class CourseApprovalHistoryConfiguration : IEntityTypeConfiguration<CourseApprovalHistory>
    {
        public void Configure(EntityTypeBuilder<CourseApprovalHistory> builder)
        {
            builder.HasOne(h => h.Course)
                .WithMany()
                .HasForeignKey(h => h.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(h => new { h.CourseId, h.ReviewedAtUtc });
        }
    }
}
