using E_LearningPlatform.Application.DTOs.Cart;
using E_LearningPlatform.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task AddToCartAsync (
      int courseId,
      CancellationToken cancellationToken = default);

        Task RemoveFromCartAsync (
            int courseId,
            CancellationToken cancellationToken = default);

        Task<CartResponseDto>
            GetCartAsync (
                CancellationToken cancellationToken = default);

        Task<OrderResponseDto>
            CheckoutAsync (
                CancellationToken cancellationToken = default);
    }
}
