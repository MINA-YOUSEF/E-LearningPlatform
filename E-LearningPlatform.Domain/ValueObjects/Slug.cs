using System;
using System.Text.RegularExpressions;

namespace E_LearningPlatform.Domain.ValueObjects
{
    public class Slug
    {
        private static readonly Regex Pattern = new Regex("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);
        public string Value { get; private set; }

        private Slug() { }

        public Slug(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Slug is required.", nameof(value));
            var normalized = value.Trim().ToLowerInvariant();
            if (!Pattern.IsMatch(normalized)) throw new ArgumentException("Slug must be lowercase letters, numbers, and dashes.", nameof(value));
            Value = normalized;
        }

        public override string ToString() => Value;
    }
}
