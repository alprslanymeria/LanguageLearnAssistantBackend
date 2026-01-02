using App.Application.Features.Translation.Dtos;
using FluentValidation;

namespace App.Application.Features.Translation.Validators;

/// <summary>
/// VALIDATOR FOR TRANSLATE TEXT REQUEST.
/// </summary>
public class TranslateTextRequestValidator : AbstractValidator<TranslateTextRequest>
{
    private static readonly HashSet<string> ValidPracticeTypes = ["reading", "listening", "writing"];

    public TranslateTextRequestValidator()
    {
        RuleFor(x => x.SelectedText)
            .NotEmpty().WithMessage("SELECTED TEXT IS REQUIRED")
            .MaximumLength(5000).WithMessage("SELECTED TEXT CANNOT EXCEED 5000 CHARACTERS");

        RuleFor(x => x.Practice)
            .NotEmpty().WithMessage("PRACTICE TYPE IS REQUIRED")
            .Must(practice => ValidPracticeTypes.Contains(practice?.ToLowerInvariant() ?? ""))
            .WithMessage("PRACTICE TYPE MUST BE ONE OF: READING, LISTENING, WRITING");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("LANGUAGE IS REQUIRED FOR WRITING PRACTICE")
            .When(x => x.Practice?.ToLowerInvariant() == "writing");
    }
}
