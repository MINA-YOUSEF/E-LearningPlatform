using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class WishListConfiguration : IEntityTypeConfiguration<WishList>
    {
        public void Configure(EntityTypeBuilder<WishList> builder)
        {
            builder.Property(w => w.Name).IsRequired().HasMaxLength(100);
            builder.HasOne<AppUser>()
                .WithOne(u => u.WishList!)
                .HasForeignKey<WishList>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
