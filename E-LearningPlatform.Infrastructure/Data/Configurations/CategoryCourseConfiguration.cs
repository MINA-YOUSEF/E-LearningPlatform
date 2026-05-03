using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_LearningPlatform.Infrastructure.Data.Configurations
{
    public class CategoryCourseConfiguration : IEntityTypeConfiguration<CategoryCourse>
    {
        public void Configure(EntityTypeBuilder<CategoryCourse> builder)
        {
            builder.HasIndex(cc => new { cc.CategoryId, cc.CourseId }).IsUnique();

            builder.HasOne(cc => cc.Course)
                .WithMany(c => c.CategoryCourses)
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Category)
                .WithMany(c => c.CategoryCourses)
                .HasForeignKey(cc => cc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
