using FluentValidation;

namespace App.Application.Features.DeckWords.Commands.CreateDeckWord;

/// <summary>
/// VALIDATOR FOR CREATE DECK WORD COMMAND.
/// </summary>
public class CreateDeckWordCommandValidator : AbstractValidator<CreateDeckWordCommand>
{
    public CreateDeckWordCommandValidator()
    {
        RuleFor(x => x.FlashcardCategoryId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD CATEGORY ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage("QUESTION IS REQUIRED")
            .MaximumLength(500)
            .WithMessage("QUESTION MUST NOT EXCEED 500 CHARACTERS");

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessage("ANSWER IS REQUIRED")
            .MaximumLength(500)
            .WithMessage("ANSWER MUST NOT EXCEED 500 CHARACTERS");
    }
}
