using FluentValidation;

namespace App.Application.Features.DeckWords.Queries.GetAllDWordsWithPaging;

/// <summary>
/// VALIDATOR FOR GET ALL DECK WORDS WITH PAGING QUERY.
/// </summary>
public class GetAllDWordsWithPagingQueryValidator : AbstractValidator<GetAllDWordsWithPagingQuery>
{
    public GetAllDWordsWithPagingQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CATEGORY ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Request.Page)
            .GreaterThan(0)
            .WithMessage("PAGE MUST BE GREATER THAN 0");

        RuleFor(x => x.Request.PageSize)
            .GreaterThan(0)
            .WithMessage("PAGE SIZE MUST BE GREATER THAN 0")
            .LessThanOrEqualTo(100)
            .WithMessage("PAGE SIZE MUST NOT EXCEED 100");
    }
}
