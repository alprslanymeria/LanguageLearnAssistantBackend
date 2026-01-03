using FluentValidation;

namespace App.Application.Features.ReadingBooks.Queries.GetRBookCreateItems;

/// <summary>
/// VALIDATOR FOR GET READING BOOK CREATE ITEMS QUERY.
/// </summary>
public class GetRBookCreateItemsQueryValidator : AbstractValidator<GetRBookCreateItemsQuery>
{
    public GetRBookCreateItemsQueryValidator()
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
