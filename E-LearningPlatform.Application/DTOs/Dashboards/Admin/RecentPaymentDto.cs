namespace E_LearningPlatform.Application.DTOs.Dashboards.Admin
{
    public class RecentPaymentDto
    {
        public int PaymentId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = null!;

        public DateTime PaidAtUtc { get; set; }
    }
}
