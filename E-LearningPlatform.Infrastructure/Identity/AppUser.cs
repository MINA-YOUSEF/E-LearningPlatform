using E_LearningPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace E_LearningPlatform.Infrastructure.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public int? MediaId { get; set; }
        public Media? ProfileImage { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool MustChangePassword { get; set; }
        public bool IsActive { get; set; } = true;

        public WishList? WishList { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Course> ApprovedCourses { get; set; } = new List<Course>();
        public ICollection<Course>? Courses { get; set; }
        public ICollection<CourseEnrollment>? Enrollments { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<StudentQuizAttempt>? StudentQuizAttempts { get; set; }
        public ICollection<LessonProgress>? LessonProgress { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Coupon> CreatedCoupons { get; set; } = new List<Coupon>();
        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
