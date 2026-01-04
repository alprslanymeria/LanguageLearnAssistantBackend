using FluentValidation;

namespace App.Application.Features.ReadingOldSessions.Commands.CreateROS;

/// <summary>
/// VALIDATOR FOR SAVE READING OLD SESSION COMMAND.
/// </summary>
public class CreateROSCommandValidator : AbstractValidator<CreateROSCommand>
{
    public CreateROSCommandValidator()
    {
        RuleFor(x => x.Request.Id)
            .NotEmpty()
            .WithMessage("ID IS REQUIRED");

        RuleFor(x => x.Request.ReadingId)
            .GreaterThan(0)
            .WithMessage("READING ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Request.ReadingBookId)
            .GreaterThan(0)
            .WithMessage("READING BOOK ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Request.Rate)
            .InclusiveBetween(0, 100)
            .WithMessage("RATE MUST BE BETWEEN 0 AND 100");
    }
}
