namespace E_LearningPlatform.Application.DTOs.Refund
{
    public class RefundResponseDto
    {
        public int RefundId { get; set; }

        public int PaymentId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; } = null!;

        public DateTime RequestedAtUtc { get; set; }
    }
}
