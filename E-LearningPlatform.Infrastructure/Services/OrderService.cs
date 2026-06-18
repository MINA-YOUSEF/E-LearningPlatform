using AutoMapper;
using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Order;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public OrderService (
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<OrderResponseDto> CreateOrderAsync (
            CreateOrderRequestDto request,
            int userId,
            CancellationToken cancellationToken = default)
        {
            if (request == null || request.CourseIds == null || !request.CourseIds.Any())
            {
                throw new BadRequestException("No courses selected.");
            }

            if (request.CourseIds.Count != request.CourseIds.Distinct().Count())
            {
                throw new BadRequestException("Duplicate courses are not allowed.");
            }

            var courses = await _unitOfWork.Courses
                .Query()
                .Where(c => request.CourseIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            if (courses.Count != request.CourseIds.Count)
            {
                throw new BadRequestException("Some courses not found.");
            }

            var currencies = courses
                .Select(x => x.Price.Currency)
                .Distinct()
                .ToList();

            if (currencies.Count > 1)
            {
                throw new BadRequestException(
                    "All courses must use the same currency.");
            }

            if (courses.Any(x =>
                !x.IsPublished ||
                !x.IsActive ||
                x.IsDeleted))
            {
                throw new BadRequestException(
                    "Some selected courses are unavailable.");
            }

            var enrolledCourses = await _unitOfWork.Enrollments
                .Query()
                .Where(x =>
                    x.UserId == userId &&
                    request.CourseIds.Contains(x.CourseId))
                .Select(x => x.CourseId)
                .ToListAsync(cancellationToken);

            if (enrolledCourses.Any())
            {
                throw new BadRequestException(
                    "You are already enrolled in some selected courses.");
            }

            var order = new Order(userId);

            foreach (var course in courses)
            {
                var item = new OrderItem(
                    course.Id,
                    course.Title,
                    course.Slug.Value,
                    course.Price);

                order.AddItem(item);
            }

            order.CalculateTotal();

            await _unitOfWork.Orders
                .AddAsync(order, cancellationToken);

            await _unitOfWork
                .SaveChangesAsync(cancellationToken);

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task CancelOrderAsync (
            int orderId,
            CancellationToken cancellationToken = default)
        {
            var currentUserId =
                _currentUserService.UserId
                ?? throw new UnauthorizedAccessException();

            var order = await _unitOfWork.Orders
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == orderId,
                    cancellationToken);

            if (order == null)
            {
                throw new NotFoundException(
                    "Order not found.");
            }

            if (order.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this order.");
            }

            order.Cancel();

            await _unitOfWork
                .SaveChangesAsync(cancellationToken);
        }

        public async Task<OrderResponseDto>
            GetOrderByIdAsync (
            int orderId,
            CancellationToken cancellationToken = default)
        {
            var currentUserId =
                _currentUserService.UserId
                ?? throw new UnauthorizedAccessException();

            var order = await _unitOfWork.Orders
                .Query()
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(
                    x => x.Id == orderId,
                    cancellationToken);

            if (order == null)
            {
                throw new NotFoundException(
                    "Order not found.");
            }

            if (order.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this order.");
            }

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<PagedResult<OrderResponseDto>>
            GetUserOrdersAsync (
            int userId,
            CancellationToken cancellationToken = default)
        {
            const int pageNumber = 1;
            const int pageSize = 10;

            var query = _unitOfWork.Orders
                .Query()
                .AsNoTracking()
                .Include(x => x.Items)
                .Where(x => x.UserId == userId);

            var totalCount =
                await query.CountAsync(cancellationToken);

            var orders = await query
                .OrderByDescending(x => x.CreatedAtUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<OrderResponseDto>
            {
                Items = _mapper.Map<List<OrderResponseDto>>(orders),

                TotalCount = totalCount,

                PageNumber = pageNumber,

                PageSize = pageSize
            };
        }

        public async Task MarkOrderAsPaidAsync (
            int orderId,
            CancellationToken cancellationToken = default)
        {
            var order = await _unitOfWork.Orders
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == orderId,
                    cancellationToken);

            if (order == null)
            {
                throw new NotFoundException(
                    "Order not found.");
            }

            order.MarkPaid();

            await _unitOfWork
                .SaveChangesAsync(cancellationToken);
        }
    }
}