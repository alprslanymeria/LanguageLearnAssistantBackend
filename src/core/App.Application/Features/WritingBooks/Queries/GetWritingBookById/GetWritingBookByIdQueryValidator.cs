using FluentValidation;

namespace App.Application.Features.WritingBooks.Queries.GetWritingBookById;

/// <summary>
/// VALIDATOR FOR GET WRITING BOOK BY ID QUERY.
/// </summary>
public class GetWritingBookByIdQueryValidator : AbstractValidator<GetWritingBookByIdQuery>
{
    public GetWritingBookByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");
    }
}
