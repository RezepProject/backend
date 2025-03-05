using backend.Entities;
using FluentValidation;

namespace backend.Controllers.Validators
{
    public class ConfigUserValidator : AbstractValidator<CreateUserToken>
    {
        public ConfigUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId must be greater than 0.");
        }
    }

    public class ChangeConfigUserValidator : AbstractValidator<ChangeConfigUser>
    {
        public ChangeConfigUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            
            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId must be greater than 0.");
        }
    }
}