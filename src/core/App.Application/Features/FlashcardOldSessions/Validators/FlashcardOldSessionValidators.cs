using App.Application.Features.FlashcardOldSessions.Dtos;
using FluentValidation;

namespace App.Application.Features.FlashcardOldSessions.Validators;

/// <summary>
/// VALIDATOR FOR SAVE FLASHCARD OLD SESSION REQUEST.
/// </summary>
public class SaveFlashcardOldSessionRequestValidator : AbstractValidator<SaveFlashcardOldSessionRequest>
{
    public SaveFlashcardOldSessionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID IS REQUIRED");

        RuleFor(x => x.FlashcardId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.FlashcardCategoryId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD CATEGORY ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Rate)
            .InclusiveBetween(0, 100)
            .WithMessage("RATE MUST BE BETWEEN 0 AND 100.");
    }
}
