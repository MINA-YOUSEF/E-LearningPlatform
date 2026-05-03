using System;

namespace E_LearningPlatform.Domain.Entities
{
    public class CouponUsage : BaseEntity
    {
        private CouponUsage() { }

        public CouponUsage(int couponId, int userId)
        {
            CouponId = couponId;
            UserId = userId;
            UsedAtUtc = DateTime.UtcNow;
        }

        public int CouponId { get; private set; }
        public Coupon Coupon { get; private set; }
        public int UserId { get; private set; }
        public DateTime UsedAtUtc { get; private set; }
    }
}
