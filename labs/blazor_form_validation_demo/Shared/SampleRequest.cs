using FluentValidation;

namespace Shared;

public sealed class SampleRequest
{
    public string Foo { get; set; }
    public int Bar { get; set; }
    public bool Baz { get; set; }

    public sealed class Validator : ValidatorBase<SampleRequest>
    {
        public Validator()
        {
            RuleFor(s => s.Foo)
                .MinimumLength(5).WithMessage("min length 5")
                .MaximumLength(12).WithMessage("max. length 12")
                .NotNull().NotEmpty();

            RuleFor(s => s.Bar)
                .InclusiveBetween(-10, 10).WithMessage("Range (-10,10)");

            RuleFor(s => s.Baz)
                .Must(v => !v).WithMessage("cannot be set if bar is positive")
                .When(s => s.Bar >= 0);

        }
    }
}
