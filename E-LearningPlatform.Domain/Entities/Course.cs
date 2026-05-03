using System;
using System.Collections.Generic;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class Course : BaseEntity
    {
        private readonly List<Section> _sections = new();
        private readonly List<Review> _reviews = new();
        private readonly List<CategoryCourse> _categoryCourses = new();
        private readonly List<WishListCourses> _wishLists = new();
        private readonly List<PaymentItem> _paymentItems = new();
        private readonly List<Certificate> _certificates = new();
        private readonly List<Coupon> _coupons = new();
        private readonly List<CourseTag> _tags = new();

        private Course() { }

        public Course(string title, Slug slug, Money price, string language, int instructorId)
        {
            SetTitle(title);
            Slug = slug ?? throw new ArgumentNullException(nameof(slug));
            SetPrice(price);
            Language = language ?? throw new ArgumentNullException(nameof(language));
            InstructorId = instructorId;
            ApprovalStatus = ApprovalStatus.Draft;
            IsPublished = false;
            IsActive = true;
            AverageRating = 0;
            RatingCount = 0;
        }

        public string Title { get; private set; }
        public Slug Slug { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public string? Subtitle { get; private set; }
        public string Language { get; private set; }
        public string? LearningObjectives { get; private set; }
        public string? TargetAudience { get; private set; }
        public string? Prerequisites { get; private set; }
        public Money Price { get; private set; }
        public decimal? DiscountPercentage { get; private set; }
        public DateTime? DiscountEndDateUtc { get; private set; }
        public Duration? TotalDuration { get; private set; }
        public bool IsPublished { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public DateTime? ArchivedAt { get; private set; }
        public decimal AverageRating { get; private set; }
        public int RatingCount { get; private set; }
        public int InstructorId { get; set ; }
        public ApprovalStatus ApprovalStatus { get; private set; }
        public int? ApprovedByAdminId { get; private set; }
        public DateTime? ApprovedAtUtc { get; private set; }
        public int? ThumbnailId { get; private set; }
        public Media? Thumbnail { get; private set; }

        public IReadOnlyCollection<CategoryCourse> CategoryCourses => _categoryCourses.AsReadOnly();
        public IReadOnlyCollection<CourseTag> Tags => _tags.AsReadOnly();
        public IReadOnlyCollection<CourseEnrollment> Enrollments { get; private set; } = new List<CourseEnrollment>();
        public IReadOnlyCollection<Section> Sections => _sections.AsReadOnly();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
        public IReadOnlyCollection<WishListCourses> WishLists => _wishLists.AsReadOnly();
        public IReadOnlyCollection<PaymentItem> PaymentItems => _paymentItems.AsReadOnly();
        public IReadOnlyCollection<Certificate> Certificates => _certificates.AsReadOnly();
        public IReadOnlyCollection<Coupon> Coupons => _coupons.AsReadOnly();

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required", nameof(title));
            Title = title.Trim();
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required", nameof(description));
            Description = description.Trim();
        }

        public void SetPrice(Money price)
        {
            Price = price ?? throw new ArgumentNullException(nameof(price));
        }



        public void Publish(DateTime publishedAtUtc)
        {
            if (IsDeleted) throw new InvalidOperationException("Cannot publish deleted course.");
            IsPublished = true;
            PublishedAt = publishedAtUtc;
        }

        public void Archive(DateTime archivedAtUtc)
        {
            IsActive = false;
            ArchivedAt = archivedAtUtc;
        }

        public void Approve(int adminId)
        {
            ApprovalStatus = ApprovalStatus.Approved;
            ApprovedByAdminId = adminId;
            ApprovedAtUtc = DateTime.UtcNow;
        }

        public void Reject(int adminId)
        {
            ApprovalStatus = ApprovalStatus.Rejected;
            ApprovedByAdminId = adminId;
            ApprovedAtUtc = DateTime.UtcNow;
        }

        public void UpdateRating(Rating rating)
        {
            RatingCount += 1;
            AverageRating = ((AverageRating * (RatingCount - 1)) + rating.Value) / RatingCount;
        }

        public void SetThumbnail(int mediaId)
        {
            ThumbnailId = mediaId;
        }
    }
}
