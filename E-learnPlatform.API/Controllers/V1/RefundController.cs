namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.DTOs.Refund;
    using E_LearningPlatform.Application.Interfaces.Services;
    using E_LearningPlatform.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public partial class WishlistController
    {
        [Authorize]
        public class RefundController : BaseV1Controller
        {
            private readonly IRefundService _refundService;

            public RefundController (
                IRefundService refundService)
            {
                _refundService = refundService;
            }


            [HttpPost]
            public async Task<ActionResult<RefundResponseDto>>
                RequestRefund (
                [FromBody] CreateRefundRequestDto request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _refundService
                    .RequestRefundAsync(
                        request,
                        cancellationToken);

                return Ok(result);
            }


            [Authorize(Policy = Policies.AdminFullAccess)]
            [HttpGet("pending")]
            public async Task<ActionResult<List<RefundResponseDto>>>
                GetPendingRefunds (
                CancellationToken cancellationToken)
            {
                var result =
                    await _refundService
                    .GetPendingRefundsAsync(
                        cancellationToken);

                return Ok(result);
            }


            [Authorize(Policy = Policies.AdminFullAccess)]
            [HttpPatch("{refundId:int}/approve")]
            public async Task<IActionResult>
                Approve (
                int refundId,
                CancellationToken cancellationToken)
            {
                await _refundService
                    .ApproveRefundAsync(
                        refundId,
                        cancellationToken);

                return NoContent();
            }



            [Authorize(Policy = Policies.AdminFullAccess)]
            [HttpPatch("{refundId:int}/reject")]
            public async Task<IActionResult>
                Reject (
                int refundId,
                [FromBody] RejectRefundRequestDto request,
                CancellationToken cancellationToken)
            {
                await _refundService
                    .RejectRefundAsync(
                        refundId,
                        request.Note,
                        cancellationToken);

                return NoContent();
            }
        }
    }
    }
