using FluentValidation;

namespace App.Application.Features.ReadingOldSessions.Queries.GetROSWithPaging;

/// <summary>
/// VALIDATOR FOR GET READING OLD SESSIONS WITH PAGING QUERY.
/// </summary>
public class GetROSWithPagingQueryValidator : AbstractValidator<GetROSWithPagingQuery>
{
    public GetROSWithPagingQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.Language)
            .NotEmpty()
            .WithMessage("LANGUAGE IS REQUIRED");

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
