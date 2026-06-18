using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Entities.E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;

namespace E_LearningPlatform.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Section> Sections => Set<Section>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Media> Media => Set<Media>();
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<CourseEnrollment> Enrollments => Set<CourseEnrollment>();
        public DbSet<CourseProgress> CourseProgress => Set<CourseProgress>();
        public DbSet<LessonProgress> LessonProgress => Set<LessonProgress>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<PaymentItem> PaymentItems => Set<PaymentItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Refund> Refunds => Set<Refund>();
        public DbSet<InstructorPayout> InstructorPayouts => Set<InstructorPayout>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Certificate> Certificates => Set<Certificate>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<CategoryCourse> CategoryCourses => Set<CategoryCourse>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<CourseTag> CourseTags => Set<CourseTag>();
        public DbSet<WishList> WishLists => Set<WishList>();
        public DbSet<WishListCourses> WishListCourses => Set<WishListCourses>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Coupon> Coupons => Set<Coupon>();
        public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();
        public DbSet<CourseApprovalHistory> CourseApprovalHistories => Set<CourseApprovalHistory>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<OnlineUser> OnlineUsers => Set<OnlineUser>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Notification> Notifications => Set<Notification>();


        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            ApplySoftDeleteFilter(builder);
        }

        public override async Task<int> SaveChangesAsync (CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(BaseEntity.UpdatedAtUtc)).CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(BaseEntity.CreatedAtUtc)).CurrentValue = DateTime.UtcNow;
                    entry.Property(nameof(BaseEntity.IsDeleted)).CurrentValue = false;
                }
            }
            var domainEvents = ChangeTracker.Entries<BaseEntity>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                var outboxMessage =
               new OutboxMessage(
            domainEvent.GetType().Name,
            JsonSerializer.Serialize(
                domainEvent));

                OutboxMessages.Add(
                    outboxMessage);

            }
            foreach (var entry in ChangeTracker
            .Entries<BaseEntity>())
            {
                entry.Entity.ClearDomainEvents();
            }
            var result = await base.SaveChangesAsync(cancellationToken);


            //await _dispatcher.DispatchAsync(
            //    domainEvents);

            return result;

        }

        private static void ApplySoftDeleteFilter (ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType) && !entityType.IsOwned())
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var condition = Expression.Equal(prop, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);
                    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}
