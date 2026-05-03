using System;

namespace E_LearningPlatform.Domain.ValueObjects
{
    public class Amount
    {
        public decimal Value { get; private set; }

        private Amount() { }

        public Amount(decimal value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Amount cannot be negative.");
            Value = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public Amount Add(Amount other) => new Amount(Value + other.Value);
        public Amount Subtract(Amount other) => new Amount(Value - other.Value);
    }
}
