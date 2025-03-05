using FluentValidation;
using backend.Entities;

namespace backend.Controllers.Validators
{
    public class CreateTaskValidator : AbstractValidator<CreateTask>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Task text cannot be empty")
                .MaximumLength(500).WithMessage("Task text cannot exceed 500 characters");

            RuleFor(x => x.Done)
                .NotNull().WithMessage("Task status (Done) must be specified");
        }
    }

    public class UpdateTaskValidator : AbstractValidator<UpdateTask>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Task text cannot be empty")
                .MaximumLength(500).WithMessage("Task text cannot exceed 500 characters");

            RuleFor(x => x.Done)
                .NotNull().WithMessage("Task status (Done) must be specified");
        }
    }
}