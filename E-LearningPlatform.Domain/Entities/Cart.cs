using System.Collections.Generic;

namespace E_LearningPlatform.Domain.Entities
{
    public class Cart : BaseEntity
    {
        private readonly List<CartItem> _cartItems = new();
        private Cart() { }

        public Cart(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();
    }
}
