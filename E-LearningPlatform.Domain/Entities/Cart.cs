using System.Collections.Generic;

namespace E_LearningPlatform.Domain.Entities
{
    public class Cart : BaseEntity
    {
        private readonly List<CartItem> _cartItems = new();
        private Cart () { }

        public Cart (int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();
        public void AddItem (int courseId, decimal price)
        {
            if (_cartItems.Any(
                x => x.CourseId == courseId))
            {
                throw new InvalidOperationException(
                    "Course already exists in cart.");
            }

            _cartItems.Add(
                new CartItem(
                    courseId,
                    price));
        }

        public void RemoveItem (int courseId)
        {
            var item =
                _cartItems.FirstOrDefault(
                    x => x.CourseId == courseId);

            if (item == null)
                return;

            _cartItems.Remove(item);
        }

        public void Clear ()
        {
            _cartItems.Clear();
        }
    }
}
