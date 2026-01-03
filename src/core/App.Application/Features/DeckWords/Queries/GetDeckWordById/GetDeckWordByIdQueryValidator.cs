using FluentValidation;

namespace App.Application.Features.DeckWords.Queries.GetDeckWordById;

/// <summary>
/// VALIDATOR FOR GET DECK WORD BY ID QUERY.
/// </summary>
public class GetDeckWordByIdQueryValidator : AbstractValidator<GetDeckWordByIdQuery>
{
    public GetDeckWordByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");
    }
}
