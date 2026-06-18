using System.Collections.Generic;

namespace E_LearningPlatform.Domain.Entities
{
    public class WishList : BaseEntity
    {
        private readonly List<WishListCourses> _courses = new();
        private WishList () { }

        public WishList (int userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public int UserId { get; private set; }
        public string Name { get; private set; } = null!;
        public void AddCourse (
     int courseId)
        {
            if (_courses.Any(
                x => x.CourseId == courseId))
            {
                throw new InvalidOperationException(
                    "Course already exists in wishlist.");
            }

            _courses.Add(
                new WishListCourses(
                    courseId));
        }
        public void RemoveCourse (int courseId)
        {
            var course = _courses.Find(x => x.CourseId == courseId);
            if (course != null)
            {
                _courses.Remove(course);
            }
        }
        public IReadOnlyCollection<WishListCourses> Courses => _courses.AsReadOnly();
    }
}
