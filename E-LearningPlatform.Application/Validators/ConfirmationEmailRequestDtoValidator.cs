using E_LearningPlatform.Application.DTOs.Auth;
using FluentValidation;


namespace E_LearningPlatform.Application.Validators
{
    public class ConfirmationEmailRequestDtoValidator : AbstractValidator<ConfirmationEmailRequestDto>
    {
        public ConfirmationEmailRequestDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
