using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.AuditLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IAuditService
    {

        Task LogAsync (
        string action,
        string entityName,
        int entityId,
        object? oldValues,
        object? newValues,
        CancellationToken cancellationToken = default);

        Task<PagedResult<AuditLogDto>>
        GetLogsAsync (
            PagedRequest request,
            CancellationToken cancellationToken = default);

    }
}
