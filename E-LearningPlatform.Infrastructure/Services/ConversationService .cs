using AutoMapper;
using E_LearningPlatform.Application.DTOs.Conversation;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities.E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        
     public ConversationService (
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ConversationDto> CreateAsync (
            int instructorId,
            int courseId,
            CancellationToken cancellationToken = default)
        {
            var studentId = _currentUser.UserId!.Value;

            var existing =
                await _unitOfWork.Conversations
                .Query()
                .Include(x => x.Course)
                .FirstOrDefaultAsync(
                    x =>
                        x.StudentId == studentId
                        &&
                        x.InstructorId == instructorId
                        &&
                        x.CourseId == courseId,
                    cancellationToken);

            if (existing != null)
            {
                return _mapper.Map<ConversationDto>(
                    existing);
            }

            var conversation =
                new Conversation(
                    studentId,
                    instructorId,
                    courseId);

            await _unitOfWork.Conversations
                .AddAsync(
                    conversation,
                    cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return _mapper.Map<ConversationDto>(
                conversation);
        }

        public async Task<List<ConversationDto>>
            GetMyConversationsAsync (
            CancellationToken cancellationToken = default)
        {
            var userId =
                _currentUser.UserId!.Value;

            var conversations =
                await _unitOfWork.Conversations
                .Query()
                .Include(x => x.Course)
                .Where(x =>

                    x.StudentId == userId

                    ||

                    x.InstructorId == userId)
                .OrderByDescending(
                    x => x.LastMessageAt)
                .ToListAsync(
                    cancellationToken);

            return _mapper.Map<
                List<ConversationDto>>(
                    conversations);
        }

        public async Task<ConversationDto>
            GetByIdAsync (
            int conversationId,
            CancellationToken cancellationToken = default)
        {
            var userId =
                _currentUser.UserId!.Value;

            var conversation =
                await _unitOfWork.Conversations
                .Query()
                .Include(x => x.Course)
                .Include(x => x.Messages)
                .FirstOrDefaultAsync(
                    x => x.Id == conversationId,
                    cancellationToken);

            if (conversation == null)
            {
                throw new InvalidOperationException(
                    "Conversation not found.");
            }

            if (
                conversation.StudentId != userId
                &&
                conversation.InstructorId != userId)
            {
                throw new UnauthorizedAccessException(
                    "Access denied.");
            }

            return _mapper.Map<ConversationDto>(
                conversation);
        }

        public async Task CloseAsync (
            int conversationId,
            CancellationToken cancellationToken = default)
        {
            var userId =
                _currentUser.UserId!.Value;

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
                conversation.StudentId != userId
                &&
                conversation.InstructorId != userId)
            {
                throw new UnauthorizedAccessException(
                    "Access denied.");
            }

            if (conversation.IsClosed)
            {
                return;
            }

            conversation.Close();

            await _unitOfWork
                .SaveChangesAsync(
                    cancellationToken);
        }
    }
 
}
