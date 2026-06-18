using E_LearningPlatform.Application.DTOs.Refund;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IRefundService
    {
        Task<RefundResponseDto>
        RequestRefundAsync (
            CreateRefundRequestDto request,
            CancellationToken cancellationToken = default);

        Task ApproveRefundAsync (
            int refundId,
            CancellationToken cancellationToken = default);

        Task RejectRefundAsync (
            int refundId,
            string note,
            CancellationToken cancellationToken = default);

        Task<List<RefundResponseDto>>
            GetPendingRefundsAsync (
                CancellationToken cancellationToken = default);
    }
}
