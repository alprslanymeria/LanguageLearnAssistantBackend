using FluentValidation;

namespace App.Application.Features.WritingOldSessions.Commands.CreateWOS;

public class CreateWOSCommandValidator : AbstractValidator<CreateWOSCommand>
{
    public CreateWOSCommandValidator()
    {
        RuleFor(x => x.Request.Id)
            .NotEmpty()
            .WithMessage("ID IS REQUIRED");

        RuleFor(x => x.Request.WritingId)
            .GreaterThan(0)
            .WithMessage("WRITING ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Request.WritingBookId)
            .GreaterThan(0)
            .WithMessage("WRITING BOOK ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Request.Rate)
            .InclusiveBetween(0, 100)
            .WithMessage("RATE MUST BE BETWEEN 0 AND 100");
    }
}
