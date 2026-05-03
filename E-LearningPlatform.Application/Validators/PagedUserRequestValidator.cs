using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Domain.Enums;
using FluentValidation;


namespace E_LearningPlatform.Application.Validators
{
    public class PagedUserRequestValidator
     : AbstractValidator<PagedUserRequest>
    {
        public PagedUserRequestValidator()
        {
            RuleFor(x => x.PageNumber)
               .GreaterThan(0)
               .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
               .InclusiveBetween(1, 50)
               .WithMessage("PageSize must be between 1 and 50.");

            RuleFor(x => x.Search)
               .MaximumLength(100)
               .When(x => !string.IsNullOrEmpty(x.Search));

            RuleFor(x => x.Role)
                .Must(x => BeValidRole(x))
                .When(x => !string.IsNullOrEmpty(x.Role))
                .WithMessage("Invalid role.");

            RuleFor(x => x.CreatedFrom)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.CreatedFrom.HasValue)
                .WithMessage("CreatedFrom cannot be in the future.");
        }

        private bool BeValidRole(string? role)
        {
            return Enum.TryParse<UserRoles>(role, true, out _);
        }
    }
}