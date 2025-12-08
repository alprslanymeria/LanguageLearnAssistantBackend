using App.Application.Features.Languages.Dtos;
using FluentValidation;

namespace App.Application.Features.Languages.Validators;

public class CreateLanguageDtoValidator : AbstractValidator<CreateLanguageDto>
{
    public CreateLanguageDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Language name is required")
            .MaximumLength(100).WithMessage("Language name cannot exceed 100 characters");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));
    }
}
