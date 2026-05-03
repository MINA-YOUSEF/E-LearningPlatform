using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class WishListCoursesConfiguration : IEntityTypeConfiguration<WishListCourses>
    {
        public void Configure(EntityTypeBuilder<WishListCourses> builder)
        {
            builder.HasIndex(w => new { w.WishListId, w.CourseId }).IsUnique();

            builder.HasOne(w => w.WishList)
                .WithMany(wl => wl.Courses)
                .HasForeignKey(w => w.WishListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Course)
                .WithMany(c => c.WishLists)
                .HasForeignKey(w => w.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
