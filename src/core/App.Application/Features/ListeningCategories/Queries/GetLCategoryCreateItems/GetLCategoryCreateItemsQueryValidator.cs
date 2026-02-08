using FluentValidation;

namespace App.Application.Features.ListeningCategories.Queries.GetLCategoryCreateItems;

/// <summary>
/// VALIDATOR FOR GET LISTENING CATEGORY CREATE ITEMS QUERY.
/// </summary>
public class GetLCategoryCreateItemsQueryValidator : AbstractValidator<GetLCategoryCreateItemsQuery>
{
    public GetLCategoryCreateItemsQueryValidator()
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
