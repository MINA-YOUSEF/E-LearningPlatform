namespace E_learnPlatform.API.Controllers.V1
{
    public class ConfirmPaymentRequestDto
    {
        public string ProviderChargeId { get; set; } = string.Empty;

        public string ReceiptUrl { get; set; } = string.Empty;
    }
}
