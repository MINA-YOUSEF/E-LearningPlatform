namespace E_LearningPlatform.Infrastructure.Options
{
    public class CurrencyOptions
    {
        public const string SectionName = "CurrencyOptions";

        public List<string> SupportedCurrencies { get; set; } = new();

        public string DefaultCurrency { get; set; } = "USD";
    }
}

