using FluentValidation;

namespace App.Application.Features.ReadingSessionRows.Queries.GetRRowsByIdWithPaging;

public class GetRRowsByIdWithPagingQueryValidator : AbstractValidator<GetRRowsByIdWithPagingQuery>
{
    public GetRRowsByIdWithPagingQueryValidator()
    {
        RuleFor(x => x.OldSessionId)
            .NotEmpty()
            .WithMessage("OLD SESSION ID IS REQUIRED");

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
