using FluentValidation;

namespace App.Application.Features.DeckWords.Commands.UpdateDeckWord;

/// <summary>
/// VALIDATOR FOR UPDATE DECK WORD COMMAND.
/// </summary>
public class UpdateDeckWordCommandValidator : AbstractValidator<UpdateDeckWordCommand>
{
    public UpdateDeckWordCommandValidator()
    {
        RuleFor(x => x.Request.ItemId)
            .GreaterThan(0)
            .WithMessage("ITEM ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Request.CategoryId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD CATEGORY ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Request.Word)
            .NotEmpty()
            .WithMessage("WORD IS REQUIRED")
            .MaximumLength(500)
            .WithMessage("WORD MUST NOT EXCEED 500 CHARACTERS");

        RuleFor(x => x.Request.Answer)
            .NotEmpty()
            .WithMessage("ANSWER IS REQUIRED")
            .MaximumLength(500)
            .WithMessage("ANSWER MUST NOT EXCEED 500 CHARACTERS");
    }
}
