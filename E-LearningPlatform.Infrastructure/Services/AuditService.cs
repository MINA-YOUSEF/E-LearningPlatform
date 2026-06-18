using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.AuditLog;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public AuditService (IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<PagedResult<AuditLogDto>> GetLogsAsync (
      PagedRequest request,
      CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.AuditLogs
                .Query()
                .OrderByDescending(x => x.CreatedAtUtc);

            var totalCount = await query.CountAsync(cancellationToken);

            var logs = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new AuditLogDto
                {
                    Action = x.Action,
                    EntityName = x.EntityName,
                    EntityId = x.EntityId,
                    OldValues = x.OldValues,
                    NewValues = x.NewValues,
                    OccurredOnUtc = x.CreatedAtUtc
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<AuditLogDto>
            {
                Items = logs,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task LogAsync (string action, string entityName, int entityId, object? oldValues, object? newValues, CancellationToken cancellationToken = default)
        {
            var auditLog = new Domain.Entities.AuditLog(
                _currentUserService.UserId,
                action,
                entityName,
               entityId,
                oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                newValues != null ? JsonSerializer.Serialize(newValues) : null
            );
            await _unitOfWork.AuditLogs.AddAsync(auditLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
