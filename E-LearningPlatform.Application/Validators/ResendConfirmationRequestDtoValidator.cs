using E_LearningPlatform.Application.DTOs.Auth;
using E_LearningPlatform.Application.DTOs.Section;
using FluentValidation;


namespace E_LearningPlatform.Application.Validators
{
    public class ResendConfirmationRequestDtoValidator : AbstractValidator<ResendConfirmationRequestDto>
    {
        public ResendConfirmationRequestDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
    public class AddSectionRequestDtoValidator : AbstractValidator<AddSectionRequestDto>
    {
        public AddSectionRequestDtoValidator()
        {
            RuleFor(x => x.CourseId).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }

}