using backend.Entities;
using FluentValidation;

namespace backend.Controllers.Validators;

public class CreateAnswerValidator : AbstractValidator<CreateAnswer>
{
    public CreateAnswerValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text cannot be empty")
            .MaximumLength(500).WithMessage("Text cannot exceed 500 characters");
        
        RuleFor(x => x.User)
            .NotEmpty().WithMessage("User cannot be empty")
            .MaximumLength(100).WithMessage("User cannot exceed 100 characters");
    }
}

public class UpdateAnswerValidator : AbstractValidator<UpdateAnswer>
{
    public UpdateAnswerValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text cannot be empty")
            .MaximumLength(500).WithMessage("Text cannot exceed 500 characters");
        
        RuleFor(x => x.User)
            .NotEmpty().WithMessage("User cannot be empty")
            .MaximumLength(100).WithMessage("User cannot exceed 100 characters");
    }
}