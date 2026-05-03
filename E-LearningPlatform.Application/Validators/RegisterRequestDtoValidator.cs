using E_LearningPlatform.Application.DTOs.Auth;
using FluentValidation;


namespace E_LearningPlatform.Application.Validators
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Role).NotEmpty().Must(role => role == "Admin" || role == "Instructor" || role == "Student")
                .WithMessage("Role must be either Admin, Instructor, or Student.");
            RuleFor(x => x.Bio).MaximumLength(100);
        }
    }
}
