using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Entities.E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace E_LearningPlatform.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork (AppDbContext context)
        {
            _context = context;
        }

        private IGenericRepository<RefreshToken>? _refreshTokens;
        public IGenericRepository<RefreshToken> RefreshTokens => _refreshTokens ??= new GenericRepository<RefreshToken>(_context);

        private IGenericRepository<Course>? _courses;
        public IGenericRepository<Course> Courses => _courses ??= new GenericRepository<Course>(_context);

        private IGenericRepository<CourseEnrollment>? _enrollments;
        public IGenericRepository<CourseEnrollment> Enrollments => _enrollments ??= new GenericRepository<CourseEnrollment>(_context);

        private IGenericRepository<Payment>? _payments;
        public IGenericRepository<Payment> Payments => _payments ??= new GenericRepository<Payment>(_context);

        private IGenericRepository<Order>? _orders;
        public IGenericRepository<Order> Orders => _orders ??= new GenericRepository<Order>(_context);

        private IGenericRepository<Review>? _reviews;
        public IGenericRepository<Review> Reviews => _reviews ??= new GenericRepository<Review>(_context);

        private IGenericRepository<WishList>? _wishLists;
        public IGenericRepository<WishList> WishLists => _wishLists ??= new GenericRepository<WishList>(_context);

        private IGenericRepository<Notification>? _notifications;
        public IGenericRepository<Notification> Notifications => _notifications ??= new GenericRepository<Notification>(_context);

        private IGenericRepository<Message>? _messages;
        public IGenericRepository<Message> Messages => _messages ??= new GenericRepository<Message>(_context);

        private IGenericRepository<Media>? _media;

        public IGenericRepository<Media> Media => _media ??= new GenericRepository<Media>(_context);
        private IGenericRepository<Section>? _sections;
        public IGenericRepository<Section> Sections => _sections ??= new GenericRepository<Section>(_context);
        private IGenericRepository<Lesson>? _lessons;
        public IGenericRepository<Lesson> Lessons => _lessons ??= new GenericRepository<Lesson>(_context);
        private IGenericRepository<LessonProgress>? _lessonsProgress;
        public IGenericRepository<LessonProgress> LessonsProgress => _lessonsProgress ??= new GenericRepository<LessonProgress>(_context);

        private IGenericRepository<CourseEnrollment>? _courseEnrollment;
        public IGenericRepository<CourseEnrollment> CourseEnrollment => _courseEnrollment ??= new GenericRepository<CourseEnrollment>(_context);
        private IGenericRepository<Certificate>? _certificates;
        public IGenericRepository<Certificate> Certificates => _certificates ??= new GenericRepository<Certificate>(_context);

        private IGenericRepository<AuditLog>? _auditLogs;
        public IGenericRepository<AuditLog> AuditLogs => _auditLogs ??= new GenericRepository<AuditLog>(_context);

        private IGenericRepository<WishListCourses>? _wishListCourses;
        public IGenericRepository<WishListCourses> WishListCourses => _wishListCourses ??= new GenericRepository<WishListCourses>(_context);
        private IGenericRepository<Cart>? _carts;
        public IGenericRepository<Cart> Carts => _carts ??= new GenericRepository<Cart>(_context);

        private IGenericRepository<CartItem>? _cartItems;
        public IGenericRepository<CartItem> CartItems => _cartItems ??= new GenericRepository<CartItem>(_context);
        private IGenericRepository<Refund>? _refunds;
        public IGenericRepository<Refund> Refunds => _refunds ??= new GenericRepository<Refund>(_context);

        private IGenericRepository<Conversation> _conversation;
        public IGenericRepository<Conversation> Conversations => _conversation ??= new GenericRepository<Conversation>(_context);
        private IGenericRepository<OnlineUser>? _onlineUsers;   
        public IGenericRepository<OnlineUser> OnlineUsers => _onlineUsers ??= new GenericRepository<OnlineUser>(_context);

        public Task<int> SaveChangesAsync (CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
