using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class PaymentItemConfiguration : IEntityTypeConfiguration<PaymentItem>
    {
        public void Configure(EntityTypeBuilder<PaymentItem> builder)
        {
            builder.Property(pi => pi.Price).HasPrecision(18, 2);

            builder.HasIndex(pi => pi.PaymentId);
            builder.HasIndex(pi => pi.CourseId);

            builder.HasOne(pi => pi.Payment)
                .WithMany(p => p.Items)
                .HasForeignKey(pi => pi.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pi => pi.Course)
                .WithMany(c => c.PaymentItems)
                .HasForeignKey(pi => pi.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
