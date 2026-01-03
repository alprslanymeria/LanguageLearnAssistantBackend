using FluentValidation;

namespace App.Application.Features.ReadingOldSessions.Queries.GetReadingOldSessionsWithPaging;

/// <summary>
/// VALIDATOR FOR GET READING OLD SESSIONS WITH PAGING QUERY.
/// </summary>
public class GetReadingOldSessionsWithPagingQueryValidator : AbstractValidator<GetReadingOldSessionsWithPagingQuery>
{
    public GetReadingOldSessionsWithPagingQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("PAGE MUST BE GREATER THAN 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("PAGE SIZE MUST BE GREATER THAN 0")
            .LessThanOrEqualTo(100)
            .WithMessage("PAGE SIZE MUST NOT EXCEED 100");
    }
}
