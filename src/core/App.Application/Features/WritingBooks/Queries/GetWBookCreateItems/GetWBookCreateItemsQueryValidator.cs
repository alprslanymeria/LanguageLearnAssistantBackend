using FluentValidation;

namespace App.Application.Features.WritingBooks.Queries.GetWBookCreateItems;

/// <summary>
/// VALIDATOR FOR GET WRITING BOOK CREATE ITEMS QUERY.
/// </summary>
public class GetWBookCreateItemsQueryValidator : AbstractValidator<GetWBookCreateItemsQuery>
{
    public GetWBookCreateItemsQueryValidator()
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
