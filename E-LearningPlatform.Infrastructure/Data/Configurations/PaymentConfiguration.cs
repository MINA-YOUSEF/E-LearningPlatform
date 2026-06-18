using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasIndex(p => p.UserId);
            builder.HasIndex(p => p.OrderId);

            builder.OwnsOne(p => p.Amount, b =>
            {
                b.Property(pv => pv.Amount).HasColumnName("PaymentAmount").IsRequired().HasPrecision(18, 2);
                b.Property(pv => pv.Currency).HasColumnName("PaymentCurrency").HasMaxLength(3).IsRequired();
            });

            builder.OwnsOne(p => p.TaxAmount, b =>
            {
                b.Property(pv => pv.Amount).HasColumnName("PaymentTaxAmount").HasPrecision(18, 2);
                b.Property(pv => pv.Currency).HasColumnName("PaymentTaxCurrency").HasMaxLength(3);
            });
            builder.Navigation(p => p.TaxAmount).IsRequired(false);

            builder.HasOne<AppUser>()
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(p => p.Items)
            //    .WithOne(i => i.Payment)
            //    .HasForeignKey(i => i.PaymentId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
