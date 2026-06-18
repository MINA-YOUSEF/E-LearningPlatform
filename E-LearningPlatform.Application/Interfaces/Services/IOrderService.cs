
using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Order;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(
            CreateOrderRequestDto request ,
            int userId ,
            CancellationToken cancellationToken =default);

        Task<OrderResponseDto> GetOrderByIdAsync(int orderId,
            CancellationToken cancellationToken = default);
        Task<PagedResult<OrderResponseDto>> GetUserOrdersAsync(int userId,
            CancellationToken cancellationToken = default);
        Task MarkOrderAsPaidAsync(int orderId,
            CancellationToken cancellationToken = default);
         Task CancelOrderAsync(int orderId, CancellationToken cancellationToken = default);



    }
}