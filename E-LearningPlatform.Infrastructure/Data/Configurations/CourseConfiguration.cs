using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Description).IsRequired();
            builder.Property(c => c.Language).IsRequired().HasMaxLength(10);
            builder.Property(c => c.DiscountPercentage).HasPrecision(5, 2);
            builder.Property(c => c.AverageRating).HasPrecision(3, 2);

            builder.OwnsOne(c => c.Slug, b =>
            {
                b.Property(p => p.Value).HasColumnName("Slug").IsRequired().HasMaxLength(200);
                b.HasIndex(p => p.Value).IsUnique();
            });

            builder.OwnsOne(c => c.Price, b =>
            {
                b.Property(p => p.Amount).HasColumnName("PriceAmount").IsRequired().HasPrecision(18, 2);
                b.Property(p => p.Currency).HasColumnName("PriceCurrency").HasMaxLength(3).IsRequired();
            });

            builder.OwnsOne(c => c.TotalDuration, b =>
            {
                b.Property(p => p.Minutes).HasColumnName("TotalDurationMinutes");
            });
            builder.Navigation(c => c.TotalDuration).IsRequired(false);

            builder.HasOne<AppUser>()
                .WithMany(u => u.Courses!)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Thumbnail)
                .WithMany()
                .HasForeignKey(c => c.ThumbnailId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Sections)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Reviews)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Enrollments)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.PaymentItems)
                .WithOne(pi => pi.Course)
                .HasForeignKey(pi => pi.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Certificates)
                .WithOne(cert => cert.Course)
                .HasForeignKey(cert => cert.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Coupons)
                .WithOne(cp => cp.Course)
                .HasForeignKey(cp => cp.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
