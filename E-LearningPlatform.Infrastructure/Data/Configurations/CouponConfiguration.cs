using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasIndex(c => c.Code).IsUnique();
            builder.Property(c => c.DiscountValue).HasPrecision(10, 2);

            builder.HasOne(c => c.Course)
                .WithMany(c => c.Coupons)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>()
                .WithMany(u => u.CreatedCoupons)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Usages)
                .WithOne(u => u.Coupon)
                .HasForeignKey(u => u.CouponId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
