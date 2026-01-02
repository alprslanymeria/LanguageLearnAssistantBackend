using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Queries;

/// <summary>
/// VALIDATOR FOR GET FLASHCARD CATEGORIES WITH PAGING QUERY.
/// </summary>
public class GetFlashcardCategoriesWithPagingQueryValidator : AbstractValidator<GetFlashcardCategoriesWithPagingQuery>
{
    public GetFlashcardCategoriesWithPagingQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.PagedRequest.Page)
            .GreaterThan(0).WithMessage("PAGE MUST BE GREATER THAN 0");

        RuleFor(x => x.PagedRequest.PageSize)
            .GreaterThan(0).WithMessage("PAGE SIZE MUST BE GREATER THAN 0")
            .LessThanOrEqualTo(100).WithMessage("PAGE SIZE CANNOT EXCEED 100");
    }
}
