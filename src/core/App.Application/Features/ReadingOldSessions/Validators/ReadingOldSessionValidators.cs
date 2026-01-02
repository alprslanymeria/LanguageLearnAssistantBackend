using App.Application.Features.ReadingOldSessions.Dtos;
using FluentValidation;

namespace App.Application.Features.ReadingOldSessions.Validators;

/// <summary>
/// VALIDATOR FOR SAVE READING OLD SESSION REQUEST.
/// </summary>
public class SaveReadingOldSessionRequestValidator : AbstractValidator<SaveReadingOldSessionRequest>
{
    public SaveReadingOldSessionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID IS REQUIRED");

        RuleFor(x => x.ReadingId)
            .GreaterThan(0)
            .WithMessage("READING ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.ReadingBookId)
            .GreaterThan(0)
            .WithMessage("READING BOOK ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Rate)
            .InclusiveBetween(0, 100)
            .WithMessage("RATE MUST BE BETWEEN 0 AND 100.");
    }
}
