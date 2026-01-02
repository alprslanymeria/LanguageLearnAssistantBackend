using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Queries;

/// <summary>
/// VALIDATOR FOR GET FLASHCARD CATEGORY BY ID QUERY.
/// </summary>
public class GetFlashcardCategoryByIdQueryValidator : AbstractValidator<GetFlashcardCategoryByIdQuery>
{
    public GetFlashcardCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID MUST BE GREATER THAN 0");
    }
}
