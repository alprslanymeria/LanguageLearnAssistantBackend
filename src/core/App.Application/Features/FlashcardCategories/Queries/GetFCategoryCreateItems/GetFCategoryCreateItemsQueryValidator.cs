using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Queries.GetFCategoryCreateItems;

/// <summary>
/// VALIDATOR FOR GET FLASHCARD CATEGORY CREATE ITEMS QUERY.
/// </summary>
public class GetFCategoryCreateItemsQueryValidator : AbstractValidator<GetFCategoryCreateItemsQuery>
{
    public GetFCategoryCreateItemsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.Language)
            .NotEmpty()
            .WithMessage("LANGUAGE IS REQUIRED");

        RuleFor(x => x.Practice)
            .NotEmpty()
            .WithMessage("PRACTICE IS REQUIRED");
    }
}
