using FluentValidation;
using backend.Entities;

namespace backend.Controllers.Validators
{
    public class CreateQuestionValidator : AbstractValidator<CreateQuestion>
    {
        public CreateQuestionValidator()
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage("Text cannot be empty");
            RuleFor(x => x.Categories).NotEmpty().WithMessage("At least one category must be selected");
            RuleForEach(x => x.Categories).SetValidator(new CreateQuestionCategoryValidator());
        }
    }

    public class CreateQuestionCategoryValidator : AbstractValidator<CreateQuestionCategory>
    {
        public CreateQuestionCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name cannot be empty")
                .MaximumLength(200).WithMessage("Category name cannot exceed 200 characters");
        }
    }
}