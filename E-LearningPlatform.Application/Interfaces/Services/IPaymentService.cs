using E_LearningPlatform.Application.DTOs.Payment;
using E_LearningPlatform.Domain.Enums;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IPaymentService {
        Task<PaymentResponseDto> CreatePaymentAsync(int orderId,PaymentProvider provider, CancellationToken cancellationToken = default);
        Task ConfirmPaymentAsync(int paymentId,string providerChargeId,string receiptUrl, CancellationToken cancellationToken = default);
        Task FailPaymentAsync(
         int paymentId,
         CancellationToken cancellationToken = default);
        //Task<PaymentResponseDto> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken = default);

        //Task<IEnumerable<PaymentResponseDto>> GetPaymentsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        //Task<PaymentResponseDto> UpdatePaymentAsync(PaymentResponseDto payment, CancellationToken cancellationToken = default);
        //Task DeletePaymentAsync(int paymentId, CancellationToken cancellationToken = default);
    }

}
