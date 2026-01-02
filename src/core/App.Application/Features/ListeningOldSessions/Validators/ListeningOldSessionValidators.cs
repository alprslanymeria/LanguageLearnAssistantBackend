using App.Application.Features.ListeningOldSessions.Dtos;
using FluentValidation;

namespace App.Application.Features.ListeningOldSessions.Validators;

/// <summary>
/// VALIDATOR FOR SAVE LISTENING OLD SESSION REQUEST.
/// </summary>
public class SaveListeningOldSessionRequestValidator : AbstractValidator<SaveListeningOldSessionRequest>
{
    public SaveListeningOldSessionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID IS REQUIRED");

        RuleFor(x => x.ListeningId)
            .GreaterThan(0)
            .WithMessage("LISTENING ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.ListeningCategoryId)
            .GreaterThan(0)
            .WithMessage("LISTENING CATEGORY ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Rate)
            .InclusiveBetween(0, 100)
            .WithMessage("RATE MUST BE BETWEEN 0 AND 100.");
    }
}
