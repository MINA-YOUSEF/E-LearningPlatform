using E_LearningPlatform.Application.DTOs.OrderItem;

namespace E_LearningPlatform.Application.DTOs.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }  
        
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!; 

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
