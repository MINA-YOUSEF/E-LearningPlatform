using E_LearningPlatform.Application.DTOs.Order;
using FluentValidation;


namespace E_LearningPlatform.Application.Validators
{
    public class CreateOrderRequestDtoValidator
        : AbstractValidator<CreateOrderRequestDto>
    {
        public CreateOrderRequestDtoValidator()
        {
            RuleFor(x => x.CourseIds)
                .NotNull()
                .WithMessage("CourseIds is required.")

                .NotEmpty()
                .WithMessage("At least one course must be selected.");



            RuleForEach(x => x.CourseIds)
                .GreaterThan(0)
                .WithMessage("Invalid course id.");
        }
    }
}
