using System;

namespace E_LearningPlatform.Domain.ValueObjects
{
    public class Duration
    {
        public int Minutes { get; private set; }

        private Duration() { }

        public Duration(int minutes)
        {
            if (minutes <= 0) throw new ArgumentOutOfRangeException(nameof(minutes), "Duration must be greater than zero minutes.");
            Minutes = minutes;
        }

        public Duration Add(Duration other) => new Duration(Minutes + other.Minutes);
    }
}
