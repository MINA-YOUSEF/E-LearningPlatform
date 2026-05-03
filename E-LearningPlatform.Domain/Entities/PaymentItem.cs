namespace E_LearningPlatform.Domain.Entities
{
    public class PaymentItem : BaseEntity
    {
        private PaymentItem() { }

        public PaymentItem(int paymentId, int courseId, decimal price)
        {
            PaymentId = paymentId;
            CourseId = courseId;
            Price = price;
        }

        public int PaymentId { get; private set; }
        public Payment Payment { get; private set; } = null!;
        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public decimal Price { get; private set; }
    }
}
