using AutoMapper;
using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Notification;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public NotificationService (
            IUnitOfWork unitOfWork,
            IHubContext<NotificationHub> hubContext,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task CreateAsync (
            int userId,
            string title,
            string message,
            NotificationType type,
            CancellationToken cancellationToken = default)
        {
            var notification = new Notification(
                userId,
                title,
                message,
                type);

            await _unitOfWork.Notifications
                .AddAsync(notification, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto =
                _mapper.Map<NotificationResponseDto>(
                    notification);

            await _hubContext
                .Clients
                .User(userId.ToString())
                .SendAsync(
                    "ReceiveNotification",
                    dto,
                    cancellationToken);

            notification.MarkDelivered();

            await _unitOfWork
                .SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetUnreadCountAsync (
            CancellationToken cancellationToken = default)
        {
            var userId =
                _currentUserService.UserId!.Value;

            return await _unitOfWork
                .Notifications
                .Query()
                .AsNoTracking()
                .CountAsync(
                    x =>
                        x.RecipientUserId == userId
                        &&
                        !x.IsRead,
                    cancellationToken);
        }

        public async Task<PagedResult<NotificationResponseDto>>
            GetUserNotificationsAsync (
            PagedRequest request,
            CancellationToken cancellationToken = default)
        {
            var userId =
                _currentUserService.UserId!.Value;

            var query =
                _unitOfWork.Notifications
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.RecipientUserId == userId);

            var totalCount =
                await query.CountAsync(
                    cancellationToken);

            var notifications =
                await query
                .OrderByDescending(
                    x => x.CreatedAtUtc)
                .Skip(
                    (request.PageNumber - 1)
                    * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<NotificationResponseDto>
            {
                Items =
                    _mapper.Map<
                        List<NotificationResponseDto>>
                        (notifications),

                TotalCount = totalCount,

                PageNumber =
                    request.PageNumber,

                PageSize =
                    request.PageSize
            };
        }

        public async Task MarkAllAsReadAsync (
            CancellationToken cancellationToken = default)
        {
            var userId =
                _currentUserService.UserId!.Value;

            var notifications =
                await _unitOfWork.Notifications
                .Query()
                .Where(x =>
                    x.RecipientUserId == userId
                    &&
                    !x.IsRead)
                .ToListAsync(cancellationToken);

            foreach (var notification in notifications)
            {
                notification.MarkRead();
            }

            await _unitOfWork
                .SaveChangesAsync(cancellationToken);
        }

        public async Task MarkAsReadAsync (
            int notificationId,
            CancellationToken cancellationToken = default)
        {
            var userId =
                _currentUserService.UserId!.Value;

            var notification =
                await _unitOfWork.Notifications
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == notificationId
                        &&
                        x.RecipientUserId == userId,
                    cancellationToken);

            if (notification == null)
            {
                throw new KeyNotFoundException(
                    "Notification not found.");
            }

            if (!notification.IsRead)
            {
                notification.MarkRead();

                await _unitOfWork
                    .SaveChangesAsync(
                        cancellationToken);
            }
        }
    }
}