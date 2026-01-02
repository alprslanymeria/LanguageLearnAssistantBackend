using FluentValidation;

namespace App.Application.Features.Practices.Queries;

/// <summary>
/// VALIDATOR FOR GET PRACTICES BY LANGUAGE QUERY.
/// </summary>
public class GetPracticesByLanguageQueryValidator : AbstractValidator<GetPracticesByLanguageQuery>
{
    public GetPracticesByLanguageQueryValidator()
    {
        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("LANGUAGE IS REQUIRED")
            .MaximumLength(50).WithMessage("LANGUAGE CANNOT EXCEED 50 CHARACTERS");
    }
}
