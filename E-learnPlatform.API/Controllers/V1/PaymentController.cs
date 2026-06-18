namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.DTOs.Payment;
    using E_LearningPlatform.Application.Interfaces.Services;
    using E_LearningPlatform.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PaymentController : BaseV1Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController (
            IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponseDto>>
            Create (
            [FromBody] CreatePaymentRequestDto request,
            CancellationToken cancellationToken)
        {
            var result =
                await _paymentService.CreatePaymentAsync(
                    request.OrderId,
                    request.Provider,
                    cancellationToken);

            return Ok(result);
        }

        [HttpPatch("{paymentId:int}/confirm")]
        public async Task<IActionResult>
            Confirm (
            int paymentId,
            [FromBody] ConfirmPaymentRequestDto request,
            CancellationToken cancellationToken)
        {
            await _paymentService.ConfirmPaymentAsync(
                paymentId,
                request.ProviderChargeId,
                request.ReceiptUrl,
                cancellationToken);

            return NoContent();
        }


        [Authorize(Policy = Policies.AdminFullAccess)]
        [HttpPatch("{paymentId:int}/fail")]
        public async Task<IActionResult>
            Fail (
            int paymentId,
            CancellationToken cancellationToken)
        {
            await _paymentService.FailPaymentAsync(
                paymentId,
                cancellationToken);

            return NoContent();
        }
    }
}
