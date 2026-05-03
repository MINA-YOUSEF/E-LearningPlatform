namespace E_LearningPlatform.Domain.Entities
{
    public class WishListCourses : BaseEntity
    {
        private WishListCourses() { }

        public WishListCourses(int wishListId, int courseId)
        {
            WishListId = wishListId;
            CourseId = courseId;
        }

        public int CourseId { get; private set; }
        public Course Course { get; private set; } = null!;
        public int WishListId { get; private set; }
        public WishList WishList { get; private set; } = null!;
    }
}
