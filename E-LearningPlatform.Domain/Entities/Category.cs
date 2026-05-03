using System.Collections.Generic;

namespace E_LearningPlatform.Domain.Entities
{
    public class Category : BaseEntity
    {
        private readonly List<CategoryCourse> _categoryCourses = new();

        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public int? ParentCategoryId { get; private set; }
        public Category? ParentCategory { get; private set; }
        public string Slug { get; private set; } = null!;
        public bool IsActive { get; private set; } = true;
        public int DisplayOrder { get; private set; }

        public IReadOnlyCollection<CategoryCourse> CategoryCourses => _categoryCourses.AsReadOnly();
    }
}
