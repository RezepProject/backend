using backend.Entities;
using FluentValidation;

namespace backend.Controllers.Validators
{
    public class CreateRoleValidator : AbstractValidator<CreateRole>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role name cannot be empty")
                .MaximumLength(100).WithMessage("Role name cannot exceed 100 characters");
        }
    }
}