using System;

namespace E_LearningPlatform.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        private Money() { }

        public Money(decimal amount, string currency)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");
            if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required.", nameof(currency));

            Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
            Currency = currency.ToUpperInvariant();
        }

        public Money Add(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount - other.Amount, Currency);
        }

        private void EnsureSameCurrency(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException("Currency mismatch.");
            }
        }
    }
}
