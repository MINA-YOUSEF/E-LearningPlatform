using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Entities.E_LearningPlatform.Domain.Entities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_LearningPlatform.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<RefreshToken> RefreshTokens { get; }
        IGenericRepository<Course> Courses { get; }
        IGenericRepository<CourseEnrollment> Enrollments { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<Section> Sections { get; }
        IGenericRepository<Lesson> Lessons { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<WishList> WishLists { get; }
        IGenericRepository<LessonProgress> LessonsProgress { get; }
        IGenericRepository<CourseEnrollment> CourseEnrollment { get; }
        IGenericRepository<Notification> Notifications { get; }
        IGenericRepository<Message> Messages { get; }
        IGenericRepository<Media> Media { get; }
        IGenericRepository<AuditLog> AuditLogs { get; }
        IGenericRepository<Certificate> Certificates { get; }
        IGenericRepository<WishListCourses> WishListCourses { get; }
        IGenericRepository<Cart> Carts { get; }
        IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<Refund> Refunds { get; }
        IGenericRepository<Conversation> Conversations { get; }
        IGenericRepository<OnlineUser> OnlineUsers { get; }
        Task<int> SaveChangesAsync (CancellationToken cancellationToken = default);

    }
}
