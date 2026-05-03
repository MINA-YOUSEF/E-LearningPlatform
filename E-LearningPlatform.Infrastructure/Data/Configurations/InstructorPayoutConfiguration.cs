using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class InstructorPayoutConfiguration : IEntityTypeConfiguration<InstructorPayout>
    {
        public void Configure(EntityTypeBuilder<InstructorPayout> builder)
        {
            builder.HasIndex(p => p.InstructorId);

            builder.OwnsOne(p => p.Amount, b =>
            {
                b.Property(pv => pv.Amount).HasColumnName("PayoutAmount").IsRequired().HasPrecision(18, 2);
                b.Property(pv => pv.Currency).HasColumnName("PayoutCurrency").HasMaxLength(3).IsRequired();
            });

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(p => p.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
