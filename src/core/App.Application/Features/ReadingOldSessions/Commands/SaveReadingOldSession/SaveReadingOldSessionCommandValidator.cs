using FluentValidation;

namespace App.Application.Features.ReadingOldSessions.Commands.SaveReadingOldSession;

/// <summary>
/// VALIDATOR FOR SAVE READING OLD SESSION COMMAND.
/// </summary>
public class SaveReadingOldSessionCommandValidator : AbstractValidator<SaveReadingOldSessionCommand>
{
    public SaveReadingOldSessionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID IS REQUIRED");

        RuleFor(x => x.ReadingId)
            .GreaterThan(0)
            .WithMessage("READING ID MUST BE GREATER THAN 0");

        RuleFor(x => x.ReadingBookId)
            .GreaterThan(0)
            .WithMessage("READING BOOK ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Rate)
            .InclusiveBetween(0, 100)
            .WithMessage("RATE MUST BE BETWEEN 0 AND 100");
    }
}
