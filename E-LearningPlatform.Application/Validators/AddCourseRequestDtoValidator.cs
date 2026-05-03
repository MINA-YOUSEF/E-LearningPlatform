using E_LearningPlatform.Application.DTOs.Course;
using FluentValidation;


namespace E_LearningPlatform.Application.Validators
{
    public class AddCourseRequestDtoValidator : AbstractValidator<AddCourseRequestDto>
    {
        public AddCourseRequestDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(20);

            RuleFor(x => x.Language)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Slug)
                .NotEmpty()
                .Matches("^[a-z0-9-]+$")
                .WithMessage("Slug must be lowercase letters, numbers, and hyphens only.");

            RuleFor(x => x.PriceAmount)
                .GreaterThan(0);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3);

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100)
                .When(x => x.DiscountPercentage.HasValue);

            RuleFor(x => x.DiscountEndDateUtc)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.DiscountEndDateUtc.HasValue);

            RuleFor(x => x.TotalDurationMinutes)
                .GreaterThan(0)
                .When(x => x.TotalDurationMinutes.HasValue);

            RuleFor(x => x.ThumbnailFile)
                .Must(file => file == null || file.Length > 0)
                .WithMessage("Thumbnail file is invalid.");

            RuleFor(x => x.ThumbnailFile)
                .Must(file => file == null || file.ContentType.StartsWith("image/"))
                .WithMessage("Thumbnail must be an image.");
        }
    }
}