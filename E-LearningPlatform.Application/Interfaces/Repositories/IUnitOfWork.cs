using E_LearningPlatform.Domain.Entities;
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
        IGenericRepository<Notification> Notifications { get; }
        IGenericRepository<Message> Messages { get; }
        IGenericRepository<Media> Media { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
