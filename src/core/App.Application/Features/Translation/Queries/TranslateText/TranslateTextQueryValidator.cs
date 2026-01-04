using FluentValidation;

namespace App.Application.Features.Translation.Queries.TranslateText;

public class TranslateTextQueryValidator : AbstractValidator<TranslateTextQuery>
{

    private static readonly HashSet<string> ValidPracticeTypes = ["reading", "listening", "writing"];
    public TranslateTextQueryValidator()
    {
        RuleFor(x => x.Request.SelectedText)
            .NotEmpty().WithMessage("SELECTED TEXT IS REQUIRED")
            .MaximumLength(5000).WithMessage("SELECTED TEXT CANNOT EXCEED 5000 CHARACTERS");

        RuleFor(x => x.Request.Practice)
            .NotEmpty().WithMessage("PRACTICE TYPE IS REQUIRED")
            .Must(practice => ValidPracticeTypes.Contains(practice?.ToLowerInvariant() ?? ""))
            .WithMessage("PRACTICE TYPE MUST BE ONE OF: READING, LISTENING, WRITING");

        RuleFor(x => x.Request.Language)
            .NotEmpty().WithMessage("LANGUAGE IS REQUIRED FOR WRITING PRACTICE")
            .When(x => x.Request.Practice?.ToLowerInvariant() == "writing");
    }
}
