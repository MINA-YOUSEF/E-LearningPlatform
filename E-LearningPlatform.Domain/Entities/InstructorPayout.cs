using System;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class InstructorPayout : BaseEntity
    {
        private InstructorPayout() { }

        public InstructorPayout(int instructorId, Money amount)
        {
            InstructorId = instructorId;
            Amount = amount;
            PayoutAtUtc = DateTime.UtcNow;
        }

        public int InstructorId { get; private set; }
        public Money Amount { get; private set; }
        public DateTime PayoutAtUtc { get; private set; }
        public string? Reference { get; private set; }
    }
}
