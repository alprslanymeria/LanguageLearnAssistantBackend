using App.Application.Features.Practices.Dtos;
using FluentValidation;

namespace App.Application.Features.Practices.Validators;

public class CreatePracticeDtoValidator : AbstractValidator<CreatePracticeDto>
{
    public CreatePracticeDtoValidator()
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0).WithMessage("Language Id must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Practice name is required")
            .MaximumLength(200).WithMessage("Practice name cannot exceed 200 characters");
    }
}
