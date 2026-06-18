namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Domain.Enums;

    public class CreatePaymentRequestDto
    {
        public int OrderId { get; set; }

        public PaymentProvider Provider { get; set; }
    }
}
