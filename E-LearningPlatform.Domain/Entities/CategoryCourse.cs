namespace E_LearningPlatform.Domain.Entities
{
    public class CategoryCourse : BaseEntity
    {
        private CategoryCourse() { }

        public CategoryCourse(int categoryId, int courseId)
        {
            CategoryId = categoryId;
            CourseId = courseId;
        }

        public int CourseId { get; private set; }
        public Course Course { get; private set; } = null!;
        public int CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;
    }
}
