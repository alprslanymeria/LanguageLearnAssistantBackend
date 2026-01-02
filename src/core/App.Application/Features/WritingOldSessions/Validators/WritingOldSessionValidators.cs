using App.Application.Features.WritingOldSessions.Dtos;
using FluentValidation;

namespace App.Application.Features.WritingOldSessions.Validators;

/// <summary>
/// VALIDATOR FOR SAVE WRITING OLD SESSION REQUEST.
/// </summary>
public class SaveWritingOldSessionRequestValidator : AbstractValidator<SaveWritingOldSessionRequest>
{
    public SaveWritingOldSessionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID IS REQUIRED.");

        RuleFor(x => x.WritingId)
            .GreaterThan(0)
            .WithMessage("WRITING ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.WritingBookId)
            .GreaterThan(0)
            .WithMessage("WRITING BOOK ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Rate)
            .InclusiveBetween(0, 100)
            .WithMessage("RATE MUST BE BETWEEN 0 AND 100.");
    }
}
