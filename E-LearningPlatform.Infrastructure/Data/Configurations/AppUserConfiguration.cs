using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users");
            builder.Property(p => p.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.MustChangePassword).HasDefaultValue(false);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Bio).HasMaxLength(100);

            builder.HasOne(x => x.WishList!)
                .WithOne()
                .HasForeignKey<WishList>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Cart!)
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Courses!)
                .WithOne()
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Enrollments!)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Payments)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Reviews!)
                .WithOne()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Notifications)
                .WithOne()
                .HasForeignKey(n => n.RecipientUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
