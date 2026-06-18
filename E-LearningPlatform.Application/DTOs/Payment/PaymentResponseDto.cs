using E_LearningPlatform.Domain.Enums;

namespace E_LearningPlatform.Application.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public decimal? TaxAmount { get; set; }

        public string Currency { get; set; } = null!;

        public string? ProviderChargeId { get; set; }

        public string? ReceiptUrl { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public string Status { get; set; } = null!;

        public string Provider { get; set; } = null!;

        public DateTime? RefundedAt { get; set; }

        public int? OrderId { get; set; }
    }
}