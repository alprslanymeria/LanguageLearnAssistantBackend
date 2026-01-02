using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Queries;

/// <summary>
/// VALIDATOR FOR GET FLASHCARD CATEGORY CREATE ITEMS QUERY.
/// </summary>
public class GetFlashcardCategoryCreateItemsQueryValidator : AbstractValidator<GetFlashcardCategoryCreateItemsQuery>
{
    public GetFlashcardCategoryCreateItemsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("LANGUAGE IS REQUIRED")
            .MaximumLength(50).WithMessage("LANGUAGE CANNOT EXCEED 50 CHARACTERS");

        RuleFor(x => x.Practice)
            .NotEmpty().WithMessage("PRACTICE IS REQUIRED")
            .MaximumLength(50).WithMessage("PRACTICE CANNOT EXCEED 50 CHARACTERS");
    }
}
