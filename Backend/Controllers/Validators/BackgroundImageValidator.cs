using System.Text.RegularExpressions;
using backend.Entities;
using FluentValidation;

namespace backend.Controllers.Validators;

public class CreateBackgroundImageValidator : AbstractValidator<CreateBackgroundImage>
{
    public CreateBackgroundImageValidator()
    {
        RuleFor(x => x.Base64Image)
            .NotEmpty().WithMessage("Image data cannot be empty.")
            .Must(BeValidBase64Image).WithMessage("Invalid base64 image format.");
    }

    private bool BeValidBase64Image(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64)) return false;

        // Regex to check if it follows a typical base64 image format (data:image/png;base64,...)
        var regex = new Regex(@"^data:image\/(png|jpeg|jpg);base64,[A-Za-z0-9+/]+={0,2}$", RegexOptions.Compiled);
        return regex.IsMatch(base64);
    }
}