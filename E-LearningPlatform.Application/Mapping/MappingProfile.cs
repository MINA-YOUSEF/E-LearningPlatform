namespace E_LearningPlatform.Application.Mapping
{
    using AutoMapper;
    using E_LearningPlatform.Application.DTOs.Certificates;
    using E_LearningPlatform.Application.DTOs.Enrollment;
    using E_LearningPlatform.Application.DTOs.Notification;
    using E_LearningPlatform.Application.DTOs.Order;
    using E_LearningPlatform.Application.DTOs.OrderItem;
    using E_LearningPlatform.Application.DTOs.Payment;
    using E_LearningPlatform.Application.DTOs.Progress;
    using E_LearningPlatform.Application.DTOs.Review;
    using E_LearningPlatform.Domain.Entities;

    public class MappingProfile : Profile
    {
        public MappingProfile ()
        {
            CreateMap<Order, OrderResponseDto>()
    .ForMember(dest => dest.TotalAmount,
        opt => opt.MapFrom(src => src.Total.Amount))

    .ForMember(dest => dest.TaxAmount,
        opt => opt.MapFrom(src => src.Tax.Amount))

    .ForMember(dest => dest.Currency,
        opt => opt.MapFrom(src => src.Total.Currency))

    .ForMember(dest => dest.Status,
        opt => opt.MapFrom(src => src.Status.ToString()))

    .ForMember(dest => dest.OrderItems,
        opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.Amount,
                    opt => opt.MapFrom(src => src.Price.Amount))

                .ForMember(dest => dest.Currency,
                    opt => opt.MapFrom(src => src.Price.Currency));

            CreateMap<Payment, PaymentResponseDto>()
    .ForMember(dest => dest.Amount,
        opt => opt.MapFrom(src => src.Amount.Amount))

    .ForMember(dest => dest.TaxAmount,
        opt => opt.MapFrom(src =>
            src.TaxAmount != null
                ? src.TaxAmount.Amount
                : (decimal?)null))

    .ForMember(dest => dest.Currency,
        opt => opt.MapFrom(src => src.Amount.Currency))

    .ForMember(dest => dest.Status,
        opt => opt.MapFrom(src => src.Status.ToString()))

    .ForMember(dest => dest.Provider,
        opt => opt.MapFrom(src => src.Provider.ToString()));

            CreateMap<CourseEnrollment, EnrollmentCourseResponseDto>()
    .ForMember(dest => dest.PriceAmount,
        opt => opt.MapFrom(src => src.Price.Amount))

    .ForMember(dest => dest.Currency,
        opt => opt.MapFrom(src => src.Price.Currency))

    .ForMember(dest => dest.Status,
        opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<Course, CourseSummaryDto>()
    .ForMember(dest => dest.Slug,
        opt => opt.MapFrom(src => src.Slug.Value));
            CreateMap<LessonProgress, LessonProgressResponseDto>()
    .ForMember(dest => dest.LessonTitle,
        opt => opt.MapFrom(src => src.Lesson.Title));

            CreateMap<CourseProgress, CourseProgressResponseDto>()
    .ForMember(dest => dest.CourseTitle,
        opt => opt.MapFrom(src => src.Course.Title));
            CreateMap<Review, ReviewResponseDto>();

            CreateMap<CreateReviewRequestDto, Review>()
                .ConstructUsing(src =>
                    new Review(
                        src.CourseId,
                        0,
                        new Domain.ValueObjects.Rating(src.Rating),
                        src.Comment,
                        src.Title));

            CreateMap<Certificate, CertificateResponseDto>()
    .ForMember(dest => dest.CourseTitle,
        opt => opt.MapFrom(src => src.Course.Title))

    .ForMember(dest => dest.FileUrl,
        opt => opt.MapFrom(src => src.CertificateFile.Url));
            CreateMap<Notification, NotificationResponseDto>();
        }
    }
}


