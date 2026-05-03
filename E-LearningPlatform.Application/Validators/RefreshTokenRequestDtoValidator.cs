using E_LearningPlatform.Application.DTOs.Auth;
using FluentValidation;


namespace E_LearningPlatform.Application.Validators
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }

}
