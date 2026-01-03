using FluentValidation;

namespace App.Application.Features.ReadingBooks.Queries.GetReadingBookById;

/// <summary>
/// VALIDATOR FOR GET READING BOOK BY ID QUERY.
/// </summary>
public class GetReadingBookByIdQueryValidator : AbstractValidator<GetReadingBookByIdQuery>
{
    public GetReadingBookByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");
    }
}
