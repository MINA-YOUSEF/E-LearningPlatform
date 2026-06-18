using AutoMapper;
using E_LearningPlatform.Application.DTOs.Message;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageService (
       IUnitOfWork unitOfWork,
       IMapper mapper,
       ICurrentUserService currentUser,
       IHubContext<ChatHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
        _hubContext = hubContext;
    }

    public async Task<MessageResponseDto> SendAsync (
        SendMessageRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUser.UserId!.Value;

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            throw new InvalidOperationException(
                "Message content cannot be empty.");
        }

        var conversation =
            await _unitOfWork.Conversations
            .GetByIdAsync(
                request.ConversationId,
                cancellationToken);

        if (conversation == null)
        {
            throw new InvalidOperationException(
                "Conversation not found.");
        }

        if (conversation.IsClosed)
        {
            throw new InvalidOperationException(
                "Conversation is closed.");
        }

        if (
            conversation.StudentId != userId &&
            conversation.InstructorId != userId)
        {
            throw new UnauthorizedAccessException(
                "You are not a participant in this conversation.");
        }

        var receiverId =
            conversation.StudentId == userId
            ? conversation.InstructorId
            : conversation.StudentId;

        var message = new Message(
            conversation.Id,
            userId,
            receiverId,
            request.Content);

        await _unitOfWork.Messages
            .AddAsync(
                message,
                cancellationToken);

        conversation.UpdateLastMessage();

        message.AddDomainEvent(
            new MessageSentEvent(
                message.SenderId,
                message.ReceiverId,
                message.Content));

        await _unitOfWork
            .SaveChangesAsync(
                cancellationToken);

        var messageDto =
            _mapper.Map<MessageResponseDto>(
                message);

        await _hubContext
            .Clients
            .User(receiverId.ToString())
            .SendAsync(
                "ReceiveMessage",
                messageDto,
                cancellationToken);

        return messageDto;
    }


    public async Task<List<MessageResponseDto>>
        GetMessagesAsync (
        int conversationId,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUser.UserId!.Value;

        var conversation =
            await _unitOfWork.Conversations
            .GetByIdAsync(
                conversationId,
                cancellationToken);

        if (conversation == null)
        {
            throw new InvalidOperationException(
                "Conversation not found.");
        }

        if (
            conversation.StudentId != userId &&
            conversation.InstructorId != userId)
        {
            throw new UnauthorizedAccessException(
                "Access denied.");
        }

        var messages =
            await _unitOfWork.Messages
            .Query()
            .AsNoTracking()
            .Where(x =>
                x.ConversationId == conversationId)
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return _mapper.Map<
            List<MessageResponseDto>>(messages);
    }


    public async Task MarkAsReadAsync (
        int messageId,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUser.UserId!.Value;

        var message =
            await _unitOfWork.Messages
            .GetByIdAsync(
                messageId,
                cancellationToken);

        if (message == null)
        {
            throw new InvalidOperationException(
                "Message not found.");
        }

        if (message.ReceiverId != userId)
        {
            throw new UnauthorizedAccessException(
                "You cannot mark this message as read.");
        }

        if (!message.IsRead)
        {
            message.MarkAsRead();


            await _unitOfWork
                .SaveChangesAsync(
                    cancellationToken);

            await _hubContext
                .Clients
                .User(message.SenderId.ToString())
                .SendAsync(
                    "MessageRead",
                    new
                    {
                        MessageId = message.Id,
                        ReadAt = message.ReadAt
                    },
                    cancellationToken);
        }
    }


    public async Task<int>
        GetUnreadCountAsync (
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Messages
            .Query()
            .CountAsync(
                x =>
                    x.ReceiverId ==
                    _currentUser.UserId
                    &&
                    !x.IsRead,
                cancellationToken);
    }

}
