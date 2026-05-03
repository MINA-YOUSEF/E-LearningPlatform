using System.Collections.Generic;

namespace E_LearningPlatform.Domain.Entities
{
    public class WishList : BaseEntity
    {
        private readonly List<WishListCourses> _courses = new();
        private WishList() { }

        public WishList(int userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public int UserId { get; private set; }
        public string Name { get; private set; } = null!;
        public IReadOnlyCollection<WishListCourses> Courses => _courses.AsReadOnly();
    }
}
