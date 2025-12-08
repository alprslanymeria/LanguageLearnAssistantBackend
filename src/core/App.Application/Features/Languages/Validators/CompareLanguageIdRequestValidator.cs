using App.Application.Features.Languages.DTOs;
using FluentValidation;

namespace App.Application.Features.Languages.Validators;

public class CompareLanguageIdRequestValidator : AbstractValidator<CompareLanguageIdRequest>
{
    public CompareLanguageIdRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.LanguageId)
            .GreaterThan(0).WithMessage("Language ID must be greater than 0.");
    }
}
