using E_LearningPlatform.Application.DTOs.Cart;
using E_LearningPlatform.Application.DTOs.CartItem;
using E_LearningPlatform.Application.DTOs.Order;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOrderService _orderService;
        public CartService (IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IOrderService orderService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _orderService = orderService;
        }

        public async Task AddToCartAsync (int courseId, CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId!.Value;
            var cart = await _unitOfWork.Carts.Query().Include(x => x.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                cart = new Cart(userId);
            var enrolled = await _unitOfWork.Enrollments.Query()
                .AnyAsync(e => e.UserId == userId && e.CourseId == courseId, cancellationToken);
            if (enrolled)
            {
                throw new InvalidOperationException("Course is already enrolled.");
            }
            var courseExists =
                await _unitOfWork.Courses
                .Query().FirstOrDefaultAsync(c => c.Id == courseId, cancellationToken);
            if (courseExists == null)
            {
                throw new InvalidOperationException("Course does not exist.");
            }
            var alreadyInCart = cart.CartItems.Any(ci => ci.CourseId == courseId);
            if (alreadyInCart)
            {
                throw new InvalidOperationException("Course is already in the cart.");
            }
            cart.AddItem(courseId, courseExists.Price.Amount);
            if (cart.Id == 0)
            {
                await _unitOfWork.Carts.AddAsync(cart, cancellationToken);
            }
            else
            {
                _unitOfWork.Carts.Update(cart);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        public async Task<OrderResponseDto> CheckoutAsync (CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId!.Value;
            var cart = await _unitOfWork.Carts.Query().Include(x => x.CartItems).FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
            if (cart == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty.");
            var order = new CreateOrderRequestDto
            {
                CourseIds = cart.CartItems.Select(ci => ci.CourseId).ToList()
            };
            var result = await _orderService.CreateOrderAsync(order, userId, cancellationToken);
            if (result == null)
                throw new InvalidOperationException("Failed to create order.");
            cart.Clear();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;

        }

        public async Task<CartResponseDto> GetCartAsync (CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId!.Value;
            var cart = await _unitOfWork.Carts
                .Query()
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x.Thumbnail)
                .FirstOrDefaultAsync(
                    c => c.UserId == userId,
                    cancellationToken); var courses = await _unitOfWork.Courses.Query().Where(c => cart.CartItems.Select(ci => ci.CourseId).Contains(c.Id)).ToListAsync(cancellationToken);
            if (cart == null)
                throw new InvalidOperationException("Cart not found.");
            return new CartResponseDto
            {
                Items = cart.CartItems
         .Select(ci => new CartItemDto
         {
             CourseId = ci.CourseId,
             Title = ci.Course.Title,
             Price = ci.Price,
             ThumbnailUrl =
                 ci.Course.Thumbnail.Url
         })
         .ToList(),

                TotalPrice = cart.CartItems
         .Sum(x => x.Price)
            };
        }

        public async Task RemoveFromCartAsync (int courseId, CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId!.Value;
            var cart = await _unitOfWork.Carts.Query().Include(x => x.CartItems).FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
            if (cart == null)
                throw new InvalidOperationException("Cart not found.");
            var item = cart.CartItems.FirstOrDefault(ci => ci.CourseId == courseId);
            if (item == null)
                throw new InvalidOperationException("Course not found in cart.");
            cart.RemoveItem(courseId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
