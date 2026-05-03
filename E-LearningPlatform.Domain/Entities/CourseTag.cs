namespace E_LearningPlatform.Domain.Entities
{
    public class CourseTag : BaseEntity
    {
        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public int TagId { get; private set; }
        public Tag Tag { get; private set; }

        private CourseTag() { }

        public CourseTag(int courseId, int tagId)
        {
            CourseId = courseId;
            TagId = tagId;
        }
    }
}
