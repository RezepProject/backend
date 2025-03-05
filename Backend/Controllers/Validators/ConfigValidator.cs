using backend.Entities;
using FluentValidation;

namespace backend.Controllers.Validators
{
    public class ConfigValidator : AbstractValidator<CreateConfig>
    {
        public ConfigValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
            
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value is required.")
                .MaximumLength(500).WithMessage("Value cannot exceed 500 characters.");
        }
    }
}