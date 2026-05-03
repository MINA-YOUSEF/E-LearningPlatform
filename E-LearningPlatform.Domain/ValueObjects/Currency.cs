using System;
using System.Globalization;

namespace E_LearningPlatform.Domain.ValueObjects
{
    public class Currency
    {
        public string Code { get; private set; }

        private Currency() { }

        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Currency code is required.", nameof(code));
            Code = code.ToUpperInvariant();
            if (Code.Length != 3) throw new ArgumentException("Currency code must be 3 letters.", nameof(code));
        }

        public override string ToString() => Code;
    }
}
