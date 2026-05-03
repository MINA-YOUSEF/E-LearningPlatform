using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        private OrderItem() { }

        public OrderItem(int orderId, int courseId, decimal price ,string currency)
        {
            OrderId = orderId;
            CourseId = courseId;
            Price = new Money(price,currency);
        }

        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public int CourseId { get; private set; }
        public Course Course { get; private set; } = null!;
        public Money Price { get; private set; }

    }
}
