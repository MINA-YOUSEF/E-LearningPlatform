using E_LearningPlatform.Application.DTOs.Lesson;
using E_LearningPlatform.Domain.Enums;
using FluentValidation;


namespace E_LearningPlatform.Application.Validators
{
    public class AddLessonRequestDtoValidator : AbstractValidator<AddLessonRequestDto>
    {
        public AddLessonRequestDtoValidator()
        {
            RuleFor(x => x.SectionId)
                .GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ContentType)
                .IsInEnum();

            RuleFor(x => x.DurationInMinutes)
                .GreaterThan(0)
                .When(x => x.ContentType == LessonContentType.Video);

            RuleFor(x => x.VideoFile)
                .NotNull()
                .When(x => x.ContentType == LessonContentType.Video)
                .WithMessage("Video file is required for video lessons.");

            RuleFor(x => x.VideoFile!.Length)
                .LessThanOrEqualTo(500 * 1024 * 1024)
                .When(x => x.VideoFile != null);

            RuleFor(x => x.VideoFile!.ContentType)
                .Must(type => type.StartsWith("video/"))
                .When(x => x.VideoFile != null);

            RuleFor(x => x.ReleaseDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.ReleaseDate.HasValue);
        }
    }
}
