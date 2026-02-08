using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Queries.GetAllFCategories;

/// <summary>
/// VALIDATOR FOR GET ALL FLASHCARD CATEGORIES QUERY.
/// </summary>
public class GetAllFCategoriesQueryValidator : AbstractValidator<GetAllFCategoriesQuery>
{
    public GetAllFCategoriesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");
    }
}
