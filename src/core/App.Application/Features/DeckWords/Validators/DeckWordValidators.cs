using App.Application.Features.DeckWords.Dtos;
using FluentValidation;

namespace App.Application.Features.DeckWords.Validators;

/// <summary>
/// VALIDATOR FOR CREATE DECK WORD REQUEST.
/// </summary>
public class CreateDeckWordRequestValidator : AbstractValidator<CreateDeckWordRequest>
{
    public CreateDeckWordRequestValidator()
    {
        RuleFor(x => x.FlashcardCategoryId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD CATEGORY ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage("QUESTION IS REQUIRED")
            .MaximumLength(500)
            .WithMessage("QUESTION MUST NOT EXCEED 500 CHARACTERS");

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessage("ANSWER IS REQUIRED")
            .MaximumLength(500)
            .WithMessage("ANSWER MUST NOT EXCEED 500 CHARACTERS.");
    }
}

/// <summary>
/// VALIDATOR FOR UPDATE DECK WORD REQUEST.
/// </summary>
public class UpdateDeckWordRequestValidator : AbstractValidator<UpdateDeckWordRequest>
{
    public UpdateDeckWordRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.FlashcardCategoryId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD CATEGORY ID MUST BE GREATER THAN 0.");

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
