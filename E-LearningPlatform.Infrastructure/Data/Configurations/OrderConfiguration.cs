using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(o => o.UserId);

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(o => o.Total, b =>
            {
                b.Property(pv => pv.Amount).HasColumnName("OrderTotalAmount").IsRequired().HasPrecision(18, 2);
                b.Property(pv => pv.Currency).HasColumnName("OrderTotalCurrency").HasMaxLength(3).IsRequired();
            });

            builder.OwnsOne(o => o.Tax, b =>
            {
                b.Property(pv => pv.Amount).HasColumnName("OrderTaxAmount").IsRequired().HasPrecision(18, 2);
                b.Property(pv => pv.Currency).HasColumnName("OrderTaxCurrency").HasMaxLength(3).IsRequired();
            });

            builder.HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
