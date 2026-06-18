using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        private OrderItem() { }

        public OrderItem( 
            int courseId,
            string courseTitle,
            string courseSlug,
            Money price)
        {
             CourseId = courseId;
            CourseTitle = courseTitle;
            CourseSlug = courseSlug;
            Price = new Money(price.Amount, price.Currency);
        }

        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public int CourseId { get; private set; }
        public Course Course { get; private set; } = null!;
        public Money Price { get; private set; }
        public string CourseTitle { get; private set; }
        public string CourseSlug { get; private set; }

    }
}
