using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class CourseEnrollmentConfiguration : IEntityTypeConfiguration<CourseEnrollment>
    {
        public void Configure(EntityTypeBuilder<CourseEnrollment> builder)
        {
            builder.HasIndex(e => new { e.CourseId, e.UserId }).IsUnique();

            builder.OwnsOne(e => e.Price, b =>
            {
                b.Property(p => p.Amount).HasColumnName("EnrollmentPriceAmount").IsRequired().HasPrecision(18, 2);
                b.Property(p => p.Currency).HasColumnName("EnrollmentPriceCurrency").HasMaxLength(3).IsRequired();
            });

            builder.Property(e => e.ProgressPercent).HasPrecision(5, 2);

            builder.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<AppUser>()
                .WithMany(u => u.Enrollments!)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
