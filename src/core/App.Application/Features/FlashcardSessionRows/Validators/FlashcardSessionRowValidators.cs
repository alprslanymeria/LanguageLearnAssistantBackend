using App.Application.Features.FlashcardSessionRows.Dtos;
using FluentValidation;

namespace App.Application.Features.FlashcardSessionRows.Validators;

/// <summary>
/// VALIDATOR FOR SAVE FLASHCARD ROWS REQUEST.
/// </summary>
public class SaveFlashcardRowsRequestValidator : AbstractValidator<SaveFlashcardRowsRequest>
{
    public SaveFlashcardRowsRequestValidator()
    {
        RuleFor(x => x.FlashcardOldSessionId)
            .NotEmpty()
            .WithMessage("FLASHCARD OLD SESSION ID IS REQUIRED.");

        RuleFor(x => x.Rows)
            .NotEmpty()
            .WithMessage("AT LEAST ONE ROW IS REQUIRED.");

        RuleForEach(x => x.Rows).SetValidator(new FlashcardRowItemRequestValidator());
    }
}

/// <summary>
/// VALIDATOR FOR FLASHCARD ROW ITEM REQUEST.
/// </summary>
public class FlashcardRowItemRequestValidator : AbstractValidator<FlashcardRowItemRequest>
{
    public FlashcardRowItemRequestValidator()
    {
        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage("QUESTION IS REQUIRED.")
            .MaximumLength(500)
            .WithMessage("QUESTION MUST NOT EXCEED 500 CHARACTERS.");

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessage("ANSWER IS REQUIRED.")
            .MaximumLength(500)
            .WithMessage("ANSWER MUST NOT EXCEED 500 CHARACTERS.");
    }
}
