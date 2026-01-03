using FluentValidation;

namespace App.Application.Features.Practices.Queries.GetPracticesByLanguage;

/// <summary>
/// VALIDATOR FOR GET PRACTICES BY LANGUAGE QUERY.
/// </summary>
public class GetPracticesByLanguageQueryValidator : AbstractValidator<GetPracticesByLanguageQuery>
{
    public GetPracticesByLanguageQueryValidator()
    {
        RuleFor(x => x.Language)
            .NotEmpty()
            .WithMessage("LANGUAGE IS REQUIRED");
    }
}
