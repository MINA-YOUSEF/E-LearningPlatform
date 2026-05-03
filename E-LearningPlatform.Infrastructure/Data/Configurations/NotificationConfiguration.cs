using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(n => n.Title).IsRequired().HasMaxLength(150);
            builder.Property(n => n.Message).IsRequired();
            builder.HasIndex(n => n.RecipientUserId);

            builder.HasOne<AppUser>()
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.RecipientUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
