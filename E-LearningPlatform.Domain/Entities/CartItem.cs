namespace E_LearningPlatform.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        private CartItem () { }

        public CartItem (int courseId, decimal price)
        {
            CourseId = courseId;
            Price = price;
        }

        public int CartId { get; private set; }
        public Cart Cart { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public decimal Price { get; private set; }
    }
}
