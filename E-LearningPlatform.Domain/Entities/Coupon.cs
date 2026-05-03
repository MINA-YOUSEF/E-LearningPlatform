using System;
using System.Collections.Generic;
using E_LearningPlatform.Domain.Enums;

namespace E_LearningPlatform.Domain.Entities
{
    public class Coupon : BaseEntity
    {
        private readonly List<CouponUsage> _usages = new();
        private Coupon() { }

        public Coupon(string code, decimal discountValue, DiscountType type, DateTime expireAtUtc, int maxUsage, int instructorId, int courseId)
        {
            Code = code;
            DiscountValue = discountValue;
            Type = type;
            ExpireAtUtc = expireAtUtc;
            MaxUsage = maxUsage;
            InstructorId = instructorId;
            CourseId = courseId;
            IsActive = true;
        }

        public string Code { get; private set; } = null!;
        public decimal DiscountValue { get; private set; }
        public DiscountType Type { get; private set; }
        public DateTime ExpireAtUtc { get; private set; }
        public int MaxUsage { get; private set; }
        public int UsedCount { get; private set; }
        public int InstructorId { get; private set; }
        public bool IsActive { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; } = null!;
        public IReadOnlyCollection<CouponUsage> Usages => _usages.AsReadOnly();

        public void IncrementUsage()
        {
            if (UsedCount >= MaxUsage) throw new InvalidOperationException("Coupon usage exceeded.");
            UsedCount++;
        }
    }
}
